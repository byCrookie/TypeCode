namespace TypeCode.Business.Mode.Guid;

public class GuidTypeCodeGeneratorParameter : ITypeCodeGeneratorParameter
{
    public GuidTypeCodeGeneratorParameter(GuidFormat format)
    {
        Format = format;
    }
    
    public GuidFormat Format { get; }
}