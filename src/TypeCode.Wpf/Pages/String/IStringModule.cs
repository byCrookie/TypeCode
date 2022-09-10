using Jab;

namespace TypeCode.Wpf.Pages.String;

[ServiceProviderModule]
[Transient(typeof(StringView))]
[Transient(typeof(StringViewModel))]
public interface IStringModule
{
    
}