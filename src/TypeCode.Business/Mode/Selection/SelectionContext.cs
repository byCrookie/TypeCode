using Framework.Workflow;

namespace TypeCode.Business.Mode.Selection
{
    internal class SelectionContext : WorkflowBaseContext, ISelectionContext
    {
        public short Selection { get; set; }
        public string Input { get; set; }
    }
}