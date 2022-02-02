using TypeCode.Business.Format;
using Workflow;

namespace TypeCode.Console.Mode.ExitOrContinue;

internal class ExitOrContinueStep<TContext> :
    IExitOrContinueStep<TContext>
    where TContext : WorkflowBaseContext, IExitOrContinueContext
{
    private readonly IWorkflowBuilder<ExitOrContinueContext> _workflowBuilder;

    public ExitOrContinueStep(IWorkflowBuilder<ExitOrContinueContext> workflowBuilder)
    {
        _workflowBuilder = workflowBuilder;
    }

    public async Task ExecuteAsync(TContext context)
    {
        var exitContext = new ExitOrContinueContext
        {
            Input = context.Input
        };
        context.MapTo(exitContext);

        var workflow = _workflowBuilder
            .IfFlow(c => string.IsNullOrEmpty(c.Input), ifFlow => ifFlow
                .WriteLine(_ => $@"{Cuts.Point()} Press enter to go to menu or space to continue")
                .IfFlow(_ => System.Console.ReadKey().Key == ConsoleKey.Enter, ifFlowLeave => ifFlowLeave
                    .StopAsync()
                )
            )
            .Build();

        var workflowContext = await workflow.RunAsync(exitContext).ConfigureAwait(false);
        workflowContext.MapTo(context);
    }

    public Task<bool> ShouldExecuteAsync(TContext context)
    {
        return context.ShouldExecuteAsync();
    }
}