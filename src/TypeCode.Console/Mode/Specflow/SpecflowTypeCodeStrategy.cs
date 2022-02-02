using TypeCode.Business.Format;
using TypeCode.Business.Mode;
using TypeCode.Business.Mode.Specflow;
using TypeCode.Business.TypeEvaluation;
using TypeCode.Console.Mode.ExitOrContinue;
using Workflow;

namespace TypeCode.Console.Mode.Specflow;

internal class SpecflowTypeCodeStrategy : ISpecflowTypeCodeStrategy
{
    private readonly IWorkflowBuilder<SpecflowContext> _workflowBuilder;
    private readonly ITypeProvider _typeProvider;
    private readonly ITypeCodeGenerator<SpecflowTypeCodeGeneratorParameter> _specflowGenerator;

    public SpecflowTypeCodeStrategy(
        IWorkflowBuilder<SpecflowContext> workflowBuilder,
        ITypeProvider typeProvider,
        ITypeCodeGenerator<SpecflowTypeCodeGeneratorParameter> specflowGenerator)
    {
        _workflowBuilder = workflowBuilder;
        _typeProvider = typeProvider;
        _specflowGenerator = specflowGenerator;
    }

    public int Number()
    {
        return 1;
    }

    public string Description()
    {
        return "Specflow Table Generation";
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
            .WriteLine(_ => $@"{Cuts.Point()} Input types seperated by ,")
            .ReadLine(context => context.Input)
            .While(context => string.IsNullOrEmpty(context.Input) || !context.Input.Split(',').Select(split => split.Trim()).Any(), step => step
                .WriteLine(_ => $@"{Cuts.Point()} Please input types seperated by ,")
                .ReadLine(context => context.Input)
                .ThenAsync<IExitOrContinueStep<SpecflowContext>>()
            )
            .Then(context => context.Types, context => _typeProvider.TryGetByNames(context.Input?.Split(',').Select(split => split.Trim()).ToList() ?? new List<string>()))
            .ThenAsync(context => context.Tables, context => _specflowGenerator.GenerateAsync(new SpecflowTypeCodeGeneratorParameter
            {
                Types = context.Types.ToList()
            }))
            .If(context => string.IsNullOrEmpty(context.Tables), _ => System.Console.WriteLine($@"{Cuts.Point()} No tables found"))
            .Build();

        var workflowContext = await workflow.RunAsync(new SpecflowContext()).ConfigureAwait(false);
        return workflowContext.Tables;
    }

    public bool IsExit()
    {
        return false;
    }
}