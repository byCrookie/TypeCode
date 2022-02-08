namespace TypeCode.Business.Mode.Composer;

public class ComposerTypeCodeGeneratorParameter : ITypeCodeGeneratorParameter
{
    public ComposerTypeCodeGeneratorParameter()
    {
        ComposerTypes = new List<ComposerType>();
    }
    
    public List<ComposerType> ComposerTypes { get; set; }
}