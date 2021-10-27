using Framework.Workflow;
using TypeCode.Console.Mode.ExitOrContinue;

namespace TypeCode.Console.Mode.Selection
{
    internal class SelectionContext : WorkflowBaseContext, ISelectionContext, IExitOrContinueContext
    {
        public short Selection { get; set; }
        public string Input { get; set; }
    }
}