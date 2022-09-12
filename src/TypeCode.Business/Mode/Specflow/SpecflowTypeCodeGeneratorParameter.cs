namespace TypeCode.Business.Mode.Specflow;

public sealed class SpecflowTypeCodeGeneratorParameter : ITypeCodeGeneratorParameter
{
    public SpecflowTypeCodeGeneratorParameter()
    {
        Types = new List<Type>();
        IncludeStrings = true;
    }
    
    public List<Type> Types { get; set; }
    public bool OnlyRequired { get; set; }
    public bool IncludeStrings { get; set; }
    public bool SortAlphabetically { get; set; }
}