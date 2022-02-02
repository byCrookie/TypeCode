using TypeCode.Console.Mode.ExitOrContinue;
using TypeCode.Console.Mode.MultipleTypes;
using Workflow;

namespace TypeCode.Console.Mode.Builder;

internal class BuilderContext : WorkflowBaseContext, IMultipleTypesSelectionContext, IExitOrContinueContext
{
    public string BuilderCode { get; set; }
    public List<Type> SelectedTypes { get; set; }
    public Type SelectedType { get; set; }
    public string Input { get; set; }
}