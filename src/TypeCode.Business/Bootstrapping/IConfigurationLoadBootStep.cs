using Framework.Autofac.Boot;
using Workflow;

namespace TypeCode.Business.Bootstrapping;

public interface IConfigurationLoadBootStep<in TContext> : IWorkflowStep<TContext> 
    where TContext : WorkflowBaseContext, IBootContext
{
}