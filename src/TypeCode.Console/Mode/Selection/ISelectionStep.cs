using Workflow;

namespace TypeCode.Console.Mode.Selection
{
    internal interface ISelectionStep<in TContext, in TOptions> : 
        IWorkflowOptionsStep<TContext, TOptions>
        where TContext : WorkflowBaseContext
    {
    }
}