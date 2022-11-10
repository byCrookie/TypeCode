using TypeCode.Business.Format;
using TypeCode.Business.Mode;
using TypeCode.Business.Mode.Builder;
using TypeCode.Business.TypeEvaluation;
using TypeCode.Console.Interactive.Mode.ExitOrContinue;
using TypeCode.Console.Interactive.Mode.MultipleTypes;
using Workflow;

namespace TypeCode.Console.Interactive.Mode.Builder;

internal class BuilderTypeCodeStrategy : IBuilderTypeCodeStrategy
{
    private readonly IWorkflowBuilder<BuilderContext> _workflowBuilder;
    private readonly ILazyTypeProviderFactory _lazyTypeProviderFactory;
    private readonly ITypeCodeGenerator<BuilderTypeCodeGeneratorParameter> _builderGenerator;

    public BuilderTypeCodeStrategy(
        IWorkflowBuilder<BuilderContext> workflowBuilder,
        ILazyTypeProviderFactory lazyTypeProviderFactory,
        ITypeCodeGenerator<BuilderTypeCodeGeneratorParameter> builderGenerator
    )
    {
        _workflowBuilder = workflowBuilder;
        _lazyTypeProviderFactory = lazyTypeProviderFactory;
        _builderGenerator = builderGenerator;
    }

    public int Number()
    {
        return 5;
    }

    public string Description()
    {
        return "Builder Generation";
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
            .WriteLine(_ => $@"{Cuts.Point()} Input type")
            .ReadLine(c => c.Input)
            .While(c => !typeProvider.HasByName(c.Input?.Trim(), ct: ct), whileFlow => whileFlow
                .WriteLine(_ => $@"{Cuts.Point()} Type not found")
                .WriteLine(_ => $@"{Cuts.Point()} Please input input type")
                .ReadLine(c => c.Input)
                .ThenAsync<IExitOrContinueStep<BuilderContext>>()
            )
            .Then(c => c.SelectedTypes, c => typeProvider.TryGetByName(c.Input?.Trim(), ct: ct).ToList())
            .ThenAsync<IMultipleTypeSelectionStep<BuilderContext>>()
            .Stop(c => !c.SelectedType?.IsClass ?? false, _ => System.Console.WriteLine($@"{Cuts.Point()} Type has to be a class"))
            .ThenAsync(c => c.BuilderCode, c => _builderGenerator.GenerateAsync(new BuilderTypeCodeGeneratorParameter
            {
                Type = c.SelectedType
            }))
            .Build();

        var workflowContext = await workflow.RunAsync(new BuilderContext()).ConfigureAwait(false);
        return workflowContext.BuilderCode;
    }

    public bool IsExit()
    {
        return false;
    }
}