using System.Text;

namespace TypeCode.Business.Mode.EncodingConversion;

public class EncodingTypeCodeGenerator : IEncodingTypeCodeGenerator
{
    public Task<string?> GenerateAsync(EncodingTypeCodeGeneratorParameter parameter)
    {
        var fromBytes = parameter.EncodingFrom.GetBytes(parameter.Input);
        var convertedBytes = Encoding.Convert(parameter.EncodingFrom, parameter.EncodingTo, fromBytes);
        return Task.FromResult<string?>(parameter.EncodingTo.GetString(convertedBytes));
    }
}