using Framework.Workflow;

namespace TypeCode.Console.Mode.MultipleTypes
{
    internal interface IMultipleTypeSelectionStep<in TContext> : 
        IWorkflowStep<TContext> 
        where TContext : WorkflowBaseContext, IMultipleTypesSelectionContext
    {

    }
}