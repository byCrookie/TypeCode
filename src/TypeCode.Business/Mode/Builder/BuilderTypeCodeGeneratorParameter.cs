namespace TypeCode.Business.Mode.Builder;

public class BuilderTypeCodeGeneratorParameter : ITypeCodeGeneratorParameter
{
    public BuilderTypeCodeGeneratorParameter()
    {
        AlreadyMapped = new List<Type>();
    }
    
    public Type? Type { get; set; }
    
    public List<Type> AlreadyMapped { get; }
    public bool Recursive { get; set; }
}