using TypeCode.Console.Interactive.Mode.ExitOrContinue;
using TypeCode.Console.Interactive.Mode.MultipleTypes;
using Workflow;

namespace TypeCode.Console.Interactive.Mode.Builder;

internal class BuilderContext : WorkflowBaseContext, IMultipleTypesSelectionContext, IExitOrContinueContext
{
    public BuilderContext()
    {
        SelectedTypes = new List<Type>();
    }
    
    public string? BuilderCode { get; set; }
    public List<Type> SelectedTypes { get; set; }
    public Type? SelectedType { get; set; }
    public string? Input { get; set; }
}