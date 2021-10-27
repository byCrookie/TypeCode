using Framework.Workflow;

namespace TypeCode.Console.Mode.MultipleTypes
{
    internal interface IMultipleTypeSelectionStep<TContext> : 
        IWorkflowStep<TContext> 
        where TContext : WorkflowBaseContext, IMultipleTypesSelectionContext
    {

    }
}