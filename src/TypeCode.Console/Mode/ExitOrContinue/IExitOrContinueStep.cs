using Workflow;

namespace TypeCode.Console.Mode.ExitOrContinue;

internal interface IExitOrContinueStep<in TContext> : 
    IWorkflowStep<TContext>
    where TContext : WorkflowBaseContext, IExitOrContinueContext
{
}