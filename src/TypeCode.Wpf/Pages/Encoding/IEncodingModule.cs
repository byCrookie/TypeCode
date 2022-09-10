using Jab;

namespace TypeCode.Wpf.Pages.Encoding;

[ServiceProviderModule]
[Transient(typeof(EncodingView))]
[Transient(typeof(EncodingViewModel))]
public interface IEncodingModule
{
    
}