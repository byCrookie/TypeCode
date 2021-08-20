using Framework.Workflow;

namespace TypeCode.Business.Mode.Selection
{
    internal interface ISelectionStep<in TContext, in TOptions> : 
        IWorkflowOptionsStep<TContext, TOptions>
        where TContext : WorkflowBaseContext
    {
    }
}