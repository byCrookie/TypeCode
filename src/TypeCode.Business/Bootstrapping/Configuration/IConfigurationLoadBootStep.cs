using Framework.Boot;
using Workflow;

namespace TypeCode.Business.Bootstrapping.Configuration;

public interface IConfigurationLoadBootStep<in TContext> : IWorkflowStep<TContext> 
    where TContext : WorkflowBaseContext, IBootContext
{
}