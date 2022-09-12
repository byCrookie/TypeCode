namespace TypeCode.Business.Mode.Guid;

public sealed class GuidTypeCodeGeneratorParameter : ITypeCodeGeneratorParameter
{
    public GuidTypeCodeGeneratorParameter(GuidFormat format)
    {
        Format = format;
    }
    
    public GuidFormat Format { get; }
}