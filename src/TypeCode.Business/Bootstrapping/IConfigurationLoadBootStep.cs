using Framework.Jab.Boot;
using Workflow;

namespace TypeCode.Business.Bootstrapping;

public interface IConfigurationLoadBootStep<in TContext> : IWorkflowStep<TContext> 
    where TContext : WorkflowBaseContext, IBootContext
{
}