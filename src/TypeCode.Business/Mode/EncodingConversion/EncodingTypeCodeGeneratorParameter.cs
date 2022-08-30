using System.Text;

namespace TypeCode.Business.Mode.EncodingConversion;

public class EncodingTypeCodeGeneratorParameter : ITypeCodeGeneratorParameter
{
    public EncodingTypeCodeGeneratorParameter(string input, Encoding encodingFrom, Encoding encodingTo)
    {
        Input = input;
        EncodingFrom = encodingFrom;
        EncodingTo = encodingTo;
    }

    public string Input { get; }
    public Encoding EncodingFrom { get; }
    public Encoding EncodingTo { get; }
}