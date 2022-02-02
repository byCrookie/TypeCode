using TypeCode.Console.Mode;
using TypeCode.Console.Mode.ExitOrContinue;
using Workflow;

namespace TypeCode.Console;

internal class TypeCodeContext : WorkflowBaseContext, IExitOrContinueContext
{
    public TypeCodeContext()
    {
        Modes = new List<ITypeCodeStrategy>();
    }
    
    public string? Input { get; set; }
    public ITypeCodeStrategy? Mode { get; set; }
    public IEnumerable<ITypeCodeStrategy> Modes { get; set; }
}