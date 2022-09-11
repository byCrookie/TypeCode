using Jab;

namespace TypeCode.Wpf.Pages.Guid;

[ServiceProviderModule]
[Singleton(typeof(GuidView))]
[Singleton(typeof(GuidViewModel))]
public interface IGuidModule
{
    
}