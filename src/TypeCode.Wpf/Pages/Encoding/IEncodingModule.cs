using Jab;

namespace TypeCode.Wpf.Pages.Encoding;

[ServiceProviderModule]
[Singleton(typeof(EncodingView))]
[Singleton(typeof(EncodingViewModel))]
public interface IEncodingModule
{
    
}