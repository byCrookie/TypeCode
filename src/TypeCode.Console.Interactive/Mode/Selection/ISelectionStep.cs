using Workflow;

namespace TypeCode.Console.Interactive.Mode.Selection;

internal interface ISelectionStep<in TContext, TOptions> : 
    IWorkflowOptionsStep<TContext, TOptions>
    where TContext : WorkflowBaseContext
{
}