using Framework.Boot;
using Framework.Workflow;

namespace TypeCode.Business.Bootstrapping
{
    public interface IAssemblyLoadBootStep<in TContext> : IWorkflowStep<TContext> 
        where TContext : WorkflowBaseContext, IBootContext
    {
    }
}