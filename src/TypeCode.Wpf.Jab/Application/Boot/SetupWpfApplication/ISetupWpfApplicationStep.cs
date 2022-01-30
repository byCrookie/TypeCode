using Framework.Autofac.Boot;
using Workflow;

namespace TypeCode.Wpf.Jab.Application.Boot.SetupWpfApplication;

public interface ISetupWpfApplicationStep<in TContext> : IWorkflowStep<TContext> 
    where TContext : WorkflowBaseContext, IBootContext
{
}