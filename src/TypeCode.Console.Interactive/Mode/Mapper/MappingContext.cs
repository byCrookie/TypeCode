using Humanizer;
using TypeCode.Business.Mode.Mapper;
using TypeCode.Console.Interactive.Mode.MultipleTypes;
using TypeCode.Console.Interactive.Mode.Selection;
using Workflow;

namespace TypeCode.Console.Interactive.Mode.Mapper;

internal class MappingContext : WorkflowBaseContext, IMultipleTypesSelectionContext, ISelectionContext
{
    public MappingContext()
    {
        TypeNames = new List<string>();
        SelectedTypes = new List<Type>();
        FirstTypeNames = new List<Type>();
        SecondTypeNames = new List<Type>();
    }
    
    public string? Input { get; set; }
    public IEnumerable<string> TypeNames { get; set; }
    public string? MappingCode { get; set; }
    public List<Type> SelectedTypes { get; set; }
    public Type? SelectedType { get; set; }
    public IEnumerable<Type> FirstTypeNames { get; set; }
    public IEnumerable<Type> SecondTypeNames { get; set; }
    public MappingType? SelectedFirstType { get; set; }
    public MappingType? SelectedSecondType { get; set; }
    public short Selection { get; set; }
}