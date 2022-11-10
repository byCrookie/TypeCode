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
    private readonly ILazyTypeProviderFactory _lazyTypeProviderFactory;
    private readonly ITypeCodeGenerator<ComposerTypeCodeGeneratorParameter> _composerGenerator;

    public ComposerTypeCodeStrategy(
        IWorkflowBuilder<ComposerContext> workflowBuilder,
        ILazyTypeProviderFactory lazyTypeProviderFactory,
        ITypeCodeGenerator<ComposerTypeCodeGeneratorParameter> composerGenerator
    )
    {
        _workflowBuilder = workflowBuilder;
        _lazyTypeProviderFactory = lazyTypeProviderFactory;
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

    public async Task<string?> GenerateAsync(CancellationToken? ct = null)
    {
        var typeProvider = await _lazyTypeProviderFactory.ValueAsync().ConfigureAwait(false);
        
        var workflow = _workflowBuilder
            .WriteLine(_ => $@"{Cuts.Point()} Input strategy interface")
            .ReadLine(c => c.Input)
            .While(c => !typeProvider.HasByName(c.Input?.Trim(), ct: ct), whileFlow => whileFlow
                .WriteLine(_ => $@"{Cuts.Point()} Interface not found")
                .WriteLine(_ => $@"{Cuts.Point()} Please input strategy interface")
                .ReadLine(c => c.Input)
                .ThenAsync<IExitOrContinueStep<ComposerContext>>()
            )
            .Then(c => c.SelectedTypes, c => typeProvider.TryGetByName(c.Input?.Trim(), ct: ct).ToList())
            .ThenAsync<IMultipleTypeSelectionStep<ComposerContext>>()
            .Stop(c => !c.SelectedType?.IsInterface ?? false, _ => System.Console.WriteLine($@"{Cuts.Point()} Type has to be an interface"))
            .ThenAsync(c => c.ComposerCode, c => _composerGenerator.GenerateAsync(new ComposerTypeCodeGeneratorParameter
            {
                ComposerTypes = new List<ComposerType>
                {
                    new(c.SelectedType ?? throw new InvalidOperationException(), c.SelectedType is not null ? typeProvider
                        .TryGetTypesByCondition(typ => typ.GetInterface(c.SelectedType.Name) != null, ct: ct)
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