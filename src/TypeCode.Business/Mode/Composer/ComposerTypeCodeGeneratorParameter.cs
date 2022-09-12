namespace TypeCode.Business.Mode.Composer;

public sealed class ComposerTypeCodeGeneratorParameter : ITypeCodeGeneratorParameter
{
    public ComposerTypeCodeGeneratorParameter()
    {
        ComposerTypes = new List<ComposerType>();
    }
    
    public List<ComposerType> ComposerTypes { get; set; }
}