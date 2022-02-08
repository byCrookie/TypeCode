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
    private readonly ITypeProvider _typeProvider;
    private readonly ITypeCodeGenerator<BuilderTypeCodeGeneratorParameter> _builderGenerator;

    public BuilderTypeCodeStrategy(
        IWorkflowBuilder<BuilderContext> workflowBuilder,
        ITypeProvider typeProvider,
        ITypeCodeGenerator<BuilderTypeCodeGeneratorParameter> builderGenerator
    )
    {
        _workflowBuilder = workflowBuilder;
        _typeProvider = typeProvider;
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

    public async Task<string?> GenerateAsync()
    {
        var workflow = _workflowBuilder
            .WriteLine(_ => $@"{Cuts.Point()} Input type")
            .ReadLine(c => c.Input)
            .While(c => !_typeProvider.HasByName(c.Input?.Trim()), whileFlow => whileFlow
                .WriteLine(_ => $@"{Cuts.Point()} Type not found")
                .WriteLine(_ => $@"{Cuts.Point()} Please input input type")
                .ReadLine(c => c.Input)
                .ThenAsync<IExitOrContinueStep<BuilderContext>>()
            )
            .Then(c => c.SelectedTypes, c => _typeProvider.TryGetByName(c.Input?.Trim()).ToList())
            .ThenAsync<IMultipleTypeSelectionStep<BuilderContext>>()
            .Stop(c => !c.SelectedType?.IsClass ?? false, _ => System.Console.WriteLine($@"{Cuts.Point()} Type has to be a class"))
            .ThenAsync(c => c.BuilderCode, c => _builderGenerator.GenerateAsync(new BuilderTypeCodeGeneratorParameter
            {
                Types = c.SelectedType is not null ? new List<Type> { c.SelectedType } : new List<Type>()
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