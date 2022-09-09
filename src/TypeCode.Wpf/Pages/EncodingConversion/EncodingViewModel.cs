using System.Text;

namespace TypeCode.Wpf.Pages.EncodingConversion;

public class EncodingViewModel
{
    public Encoding Encoding { get; }
    public EncodingInfo EncodingInfo { get; }

    public EncodingViewModel(Encoding encoding, EncodingInfo encodingInfo)
    {
        Encoding = encoding;
        EncodingInfo = encodingInfo;
    }

    public override string ToString()
    {
        return $"{EncodingInfo.Name} {Encoding.EncodingName} {EncodingInfo.DisplayName} {EncodingInfo.CodePage}";
    }
}