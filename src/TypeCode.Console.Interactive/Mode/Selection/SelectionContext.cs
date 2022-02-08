using TypeCode.Console.Interactive.Mode.ExitOrContinue;
using Workflow;

namespace TypeCode.Console.Interactive.Mode.Selection;

internal class SelectionContext : WorkflowBaseContext, ISelectionContext, IExitOrContinueContext
{
    public short Selection { get; set; }
    public string? Input { get; set; }
}