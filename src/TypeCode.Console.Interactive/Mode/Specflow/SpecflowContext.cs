using TypeCode.Console.Interactive.Mode.ExitOrContinue;
using Workflow;

namespace TypeCode.Console.Interactive.Mode.Specflow;

internal class SpecflowContext : WorkflowBaseContext, IExitOrContinueContext
{
    public SpecflowContext()
    {
        Types = new List<Type>();
    }

    public string? Input { get; set; }
    public string? Tables { get; set; }
    public IEnumerable<Type> Types { get; set; }
}