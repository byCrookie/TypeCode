using Jab;

namespace TypeCode.Wpf.Pages.Guid;

[ServiceProviderModule]
[Transient(typeof(GuidView))]
[Transient(typeof(GuidViewModel))]
public interface IGuidModule
{
    
}