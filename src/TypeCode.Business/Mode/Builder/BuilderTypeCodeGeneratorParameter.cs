namespace TypeCode.Business.Mode.Builder;

public class BuilderTypeCodeGeneratorParameter : ITypeCodeGeneratorParameter
{
    public BuilderTypeCodeGeneratorParameter()
    {
        Types = new List<Type>();
    }
    
    public IEnumerable<Type> Types { get; set; }
}