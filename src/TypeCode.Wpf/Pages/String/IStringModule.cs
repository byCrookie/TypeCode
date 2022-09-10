using Jab;
using TypeCode.Wpf.Pages.String.Escape;
using TypeCode.Wpf.Pages.String.Length;

namespace TypeCode.Wpf.Pages.String;

[ServiceProviderModule]
[Transient(typeof(StringLengthView))]
[Transient(typeof(StringLengthViewModel))]
[Transient(typeof(StringEscapeView))]
[Transient(typeof(StringEscapeViewModel))]
[Transient(typeof(StringView))]
[Transient(typeof(StringViewModel))]
public interface IStringModule
{
    
}