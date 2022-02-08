using Workflow;

namespace TypeCode.Console.Interactive.Mode.ExitOrContinue;

internal interface IExitOrContinueStep<in TContext> : 
    IWorkflowStep<TContext>
    where TContext : WorkflowBaseContext, IExitOrContinueContext
{
}