using Framework.Jab.Boot;
using Workflow;

namespace TypeCode.Business.Bootstrapping;

public interface IAssemblyLoadBootStep<in TContext> : IWorkflowStep<TContext> 
    where TContext : WorkflowBaseContext, IBootContext
{
}