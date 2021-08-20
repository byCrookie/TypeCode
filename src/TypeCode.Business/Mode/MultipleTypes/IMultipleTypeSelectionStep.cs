using Framework.Workflow;

namespace TypeCode.Business.Mode.MultipleTypes
{
    internal interface IMultipleTypeSelectionStep<TContext> : 
        IWorkflowStep<TContext> 
        where TContext : WorkflowBaseContext, IMultipleTypesSelectionContext
    {

    }
}