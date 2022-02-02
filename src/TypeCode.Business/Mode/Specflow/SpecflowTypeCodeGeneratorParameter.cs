namespace TypeCode.Business.Mode.Specflow;

public class SpecflowTypeCodeGeneratorParameter : ITypeCodeGeneratorParameter
{
    public SpecflowTypeCodeGeneratorParameter()
    {
        Types = new List<Type>();
    }
    
    public List<Type> Types { get; set; }
}