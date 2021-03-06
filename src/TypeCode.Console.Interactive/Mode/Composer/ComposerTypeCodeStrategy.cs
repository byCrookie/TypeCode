using TypeCode.Business.Format;
using TypeCode.Business.Mode;
using TypeCode.Business.Mode.Composer;
using TypeCode.Business.TypeEvaluation;
using TypeCode.Console.Interactive.Mode.ExitOrContinue;
using TypeCode.Console.Interactive.Mode.MultipleTypes;
using Workflow;

namespace TypeCode.Console.Interactive.Mode.Composer;

internal class ComposerTypeCodeStrategy : IComposerTypeCodeStrategy
{
    private readonly IWorkflowBuilder<ComposerContext> _workflowBuilder;
    private readonly ITypeProvider _typeProvider;
    private readonly ITypeCodeGenerator<ComposerTypeCodeGeneratorParameter> _composerGenerator;

    public ComposerTypeCodeStrategy(
        IWorkflowBuilder<ComposerContext> workflowBuilder,
        ITypeProvider typeProvider,
        ITypeCodeGenerator<ComposerTypeCodeGeneratorParameter> composerGenerator
    )
    {
        _workflowBuilder = workflowBuilder;
        _typeProvider = typeProvider;
        _composerGenerator = composerGenerator;
    }

    public int Number()
    {
        return 3;
    }

    public string Description()
    {
        return "Composer Generation";
    }

    public bool IsPlanned()
    {
        return false;
    }

    public bool IsBeta()
    {
        return false;
    }

    public bool IsResponsibleFor(string? mode)
    {
        return mode is not null && mode == $"{Number()}" && !IsPlanned();
    }

    public async Task<string?> GenerateAsync()
    {
        var workflow = _workflowBuilder
            .WriteLine(_ => $@"{Cuts.Point()} Input strategy interface")
            .ReadLine(c => c.Input)
            .While(c => !_typeProvider.HasByName(c.Input?.Trim()), whileFlow => whileFlow
                .WriteLine(_ => $@"{Cuts.Point()} Interface not found")
                .WriteLine(_ => $@"{Cuts.Point()} Please input strategy interface")
                .ReadLine(c => c.Input)
                .ThenAsync<IExitOrContinueStep<ComposerContext>>()
            )
            .Then(c => c.SelectedTypes, c => _typeProvider.TryGetByName(c.Input?.Trim()).ToList())
            .ThenAsync<IMultipleTypeSelectionStep<ComposerContext>>()
            .Stop(c => !c.SelectedType?.IsInterface ?? false, _ => System.Console.WriteLine($@"{Cuts.Point()} Type has to be an interface"))
            .ThenAsync(c => c.ComposerCode, c => _composerGenerator.GenerateAsync(new ComposerTypeCodeGeneratorParameter
            {
                ComposerTypes = new List<ComposerType>
                {
                    new(c.SelectedType ?? throw new InvalidOperationException(), c.SelectedType is not null ? _typeProvider
                        .TryGetTypesByCondition(typ => typ.GetInterface(c.SelectedType.Name) != null)
                        .ToList() : new List<Type>())
                }
            }))
            .Build();

        var workflowContext = await workflow.RunAsync(new ComposerContext()).ConfigureAwait(false);
        return workflowContext.ComposerCode;
    }

    public bool IsExit()
    {
        return false;
    }
}