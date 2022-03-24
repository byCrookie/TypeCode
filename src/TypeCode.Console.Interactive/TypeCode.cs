using System.Text;
using Framework.Extensions.List;
using Framework.Jab.Boot;
using TypeCode.Business.Configuration;
using TypeCode.Business.Format;
using TypeCode.Business.TypeEvaluation;
using TypeCode.Console.Interactive.Mode;
using TypeCode.Console.Interactive.Mode.ExitOrContinue;
using Workflow;

namespace TypeCode.Console.Interactive;

internal class TypeCode<TContext> : ITypeCode<TContext> where TContext : BootContext
{
    private readonly IModeComposer _modeComposer;
    private readonly IWorkflowBuilder<TypeCodeContext> _workflowBuilder;
    private readonly ITypeProvider _typeProvider;
    private readonly IConfigurationProvider _configurationProvider;

    public TypeCode(
        IModeComposer modeComposer,
        IWorkflowBuilder<TypeCodeContext> workflowBuilder,
        ITypeProvider typeProvider,
        IConfigurationProvider configurationProvider
    )
    {
        _modeComposer = modeComposer;
        _workflowBuilder = workflowBuilder;
        _typeProvider = typeProvider;
        _configurationProvider = configurationProvider;
    }

    public async Task RunAsync(TContext context, CancellationToken cancellationToken)
    {
        ITypeCodeStrategy? mode = null;

        var tasks = new List<Task>
        {
            Task.Run(InitializeTypesAsync, cancellationToken),
            Task.Run(async () => mode = await EvaluateModeAsync().ConfigureAwait(false), cancellationToken)
        };

        await Task.WhenAll(tasks).ConfigureAwait(false);
        
        while (mode is not null && !mode.IsExit())
        {
            var result = await mode.GenerateAsync().ConfigureAwait(false);
            System.Console.WriteLine(result);
            System.Console.Read();
            mode = await EvaluateModeAsync().ConfigureAwait(false);
        }

        System.Console.Read();
    }

    private async Task<ITypeCodeStrategy?> EvaluateModeAsync()
    {
        var workflow = _workflowBuilder
            .Then(context => context.Modes, _ => _modeComposer.ComposeOrdered())
            .WriteLine(context => CreateInputMessage(context.Modes))
            .ReadLine(context => context.Input)
            .While(context => !ModeExists(context.Modes, context), whileFlow => whileFlow
                .WriteLine(_ => $@"{Cuts.Point()} Please select a valid mode")
                .ReadLine(context => context.Input)
                .ThenAsync<IExitOrContinueStep<TypeCodeContext>>()
            )
            .Then(context => context.Mode, context => context.Modes
                .SingleOrDefault(strategy => strategy
                    .IsResponsibleFor(context.Input)))
            .Build();

        var workflowContext = await workflow.RunAsync(new TypeCodeContext()).ConfigureAwait(false);
        return workflowContext.Mode;
    }

    private static bool ModeExists(IEnumerable<ITypeCodeStrategy> modes, IExitOrContinueContext context)
    {
        return modes
            .Where(mode => !mode.IsPlanned())
            .Select(mode => mode.Number())
            .Any(modeNumber => $"{modeNumber}" == context.Input?.Trim());
    }

    private static string CreateInputMessage(IEnumerable<ITypeCodeStrategy> modes)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine($@"{Cuts.Long()}");
        stringBuilder.AppendLine($@"{Cuts.Heading()} Modes");
        stringBuilder.AppendLine($@"{Cuts.Long()}");
        modes.ForEach(mode => PrintModeOption(stringBuilder, mode));
        stringBuilder.AppendLine($@"{Cuts.Long()}");
        stringBuilder.Append($@"{Cuts.Point()} Select a mode");
        return stringBuilder.ToString();
    }

    private static void PrintModeOption(StringBuilder stringBuilder, ITypeCodeStrategy mode)
    {
        if (mode.IsPlanned())
        {
            stringBuilder.AppendLine($@"{Cuts.Point()} (Planned) - {mode.Description()}");
        }
        else
        {
            if (mode.IsBeta())
            {
                stringBuilder.AppendLine($@"{Cuts.Point()} {mode.Number()} - {mode.Description()} (Beta)");
            }
            else
            {
                stringBuilder.AppendLine($@"{Cuts.Point()} {mode.Number()} - {mode.Description()}");
            }
        }
    }

    private Task InitializeTypesAsync()
    {
        return _typeProvider.InitalizeAsync(_configurationProvider.GetConfiguration());
    }
}