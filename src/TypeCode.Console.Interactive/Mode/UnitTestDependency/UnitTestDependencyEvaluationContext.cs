using TypeCode.Console.Interactive.Mode.ExitOrContinue;
using TypeCode.Console.Interactive.Mode.MultipleTypes;
using TypeCode.Console.Interactive.Mode.Selection;
using Workflow;

namespace TypeCode.Console.Interactive.Mode.UnitTestDependency;

internal class UnitTestDependencyEvaluationContext : WorkflowBaseContext, ISelectionContext, IMultipleTypesSelectionContext, IExitOrContinueContext
{
    public UnitTestDependencyEvaluationContext()
    {
        SelectedTypes = new List<Type>();
    }
    
    public string? Input { get; set; }
    public string? UnitTestDependencyCode { get; set; }
    public short Selection { get; set; }
    public List<Type> SelectedTypes { get; set; }
    public Type? SelectedType { get; set; }
}