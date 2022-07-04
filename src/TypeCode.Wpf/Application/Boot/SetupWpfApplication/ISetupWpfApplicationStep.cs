using Framework.Boot;
using Workflow;

namespace TypeCode.Wpf.Application.Boot.SetupWpfApplication;

public interface ISetupWpfApplicationStep<in TContext> : IWorkflowStep<TContext> 
    where TContext : WorkflowBaseContext, IBootContext
{
}