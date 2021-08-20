using Framework.Workflow;

namespace TypeCode.Business.Mode.Specflow
{
    internal class SpecflowContext : WorkflowBaseContext
    {
        public string Input { get; set; }
        public string Tables { get; set; }
    }
}