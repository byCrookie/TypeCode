using Workflow;

namespace TypeCode.Console.Interactive.Mode.ExitOrContinue;

internal class ExitOrContinueContext : WorkflowBaseContext, IExitOrContinueContext
{
    public string? Input { get; set; }
}