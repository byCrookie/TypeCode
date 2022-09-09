using Jab;

namespace TypeCode.Wpf.Pages.EncodingConversion;

[ServiceProviderModule]
[Transient(typeof(EncodingConversionView))]
[Transient(typeof(EncodingConversionViewModel))]
public interface IEncodingConversionModule
{
    
}