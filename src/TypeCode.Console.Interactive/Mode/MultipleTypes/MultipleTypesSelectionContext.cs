using Workflow;

namespace TypeCode.Console.Interactive.Mode.MultipleTypes;

internal class MultipleTypesSelectionContext : WorkflowBaseContext, IMultipleTypesSelectionContext
{
    public MultipleTypesSelectionContext()
    {
        SelectedTypes = new List<Type>();
    }
    
    public List<Type> SelectedTypes { get; set; }
    public Type? SelectedType { get; set; }
    public string? Input { get; set; }
}