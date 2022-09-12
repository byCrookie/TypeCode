using System.Text;

namespace TypeCode.Wpf.Pages.Encoding;

public sealed class EncodingItemViewModel
{
    public System.Text.Encoding Encoding { get; }
    public EncodingInfo EncodingInfo { get; }

    public EncodingItemViewModel(System.Text.Encoding encoding, EncodingInfo encodingInfo)
    {
        Encoding = encoding;
        EncodingInfo = encodingInfo;
    }

    public override string ToString()
    {
        return $"{EncodingInfo.Name} {Encoding.EncodingName} {EncodingInfo.DisplayName} {EncodingInfo.CodePage}";
    }
}