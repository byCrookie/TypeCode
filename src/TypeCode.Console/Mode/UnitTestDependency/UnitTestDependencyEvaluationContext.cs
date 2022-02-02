using TypeCode.Console.Mode.ExitOrContinue;
using TypeCode.Console.Mode.MultipleTypes;
using TypeCode.Console.Mode.Selection;
using Workflow;

namespace TypeCode.Console.Mode.UnitTestDependency;

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