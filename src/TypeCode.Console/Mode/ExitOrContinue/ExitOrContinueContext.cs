using Workflow;

namespace TypeCode.Console.Mode.ExitOrContinue;

internal class ExitOrContinueContext : WorkflowBaseContext, IExitOrContinueContext
{
    public string? Input { get; set; }
}