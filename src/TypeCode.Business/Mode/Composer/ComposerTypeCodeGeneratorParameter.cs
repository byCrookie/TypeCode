namespace TypeCode.Business.Mode.Composer;

public class ComposerTypeCodeGeneratorParameter : ITypeCodeGeneratorParameter
{
    public ComposerTypeCodeGeneratorParameter()
    {
        Interfaces = new List<Type>();
    }
    
    public Type? Type { get; set; }
    public List<Type> Interfaces { get; set; }
}