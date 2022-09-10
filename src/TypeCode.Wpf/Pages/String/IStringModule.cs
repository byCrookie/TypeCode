using Jab;
using TypeCode.Wpf.Pages.String.Length;

namespace TypeCode.Wpf.Pages.String;

[ServiceProviderModule]
[Transient(typeof(StringLengthView))]
[Transient(typeof(StringLengthViewModel))]
[Transient(typeof(StringView))]
[Transient(typeof(StringViewModel))]
public interface IStringModule
{
    
}