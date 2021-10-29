using Framework.Boot;
using Workflow;

namespace TypeCode.Wpf.Helper.Boot.SetupWpfApplication
{
    public interface ISetupWpfApplicationStep<in TContext> : IWorkflowStep<TContext> 
        where TContext : WorkflowBaseContext, IBootContext
    {
    }
}