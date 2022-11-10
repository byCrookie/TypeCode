using Framework.Boot;
using Workflow;

namespace TypeCode.Console.Boot;

public interface ITargetDllsLoadBootStep<in TContext, TOptions> : IWorkflowOptionsStep<TContext, TOptions>
    where TContext : WorkflowBaseContext, IBootContext
{
}