using Framework.Boot;
using Workflow;

namespace TypeCode.Business.Bootstrapping.Data;

public interface IUserDataInitializeBootStep<in TContext> : IWorkflowStep<TContext> 
    where TContext : WorkflowBaseContext, IBootContext
{
}