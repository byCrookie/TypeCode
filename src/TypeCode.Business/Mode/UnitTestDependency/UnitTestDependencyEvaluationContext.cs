using Framework.Workflow;
using TypeCode.Business.Mode.Selection;

namespace TypeCode.Business.Mode.UnitTestDependency
{
    internal class UnitTestDependencyEvaluationContext : WorkflowBaseContext, ISelectionContext
    {
        public string Input { get; set; }
        public string UnitTestDependencyCode { get; set; }
        public short Selection { get; set; }
    }
}