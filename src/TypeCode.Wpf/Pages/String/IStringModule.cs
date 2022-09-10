using Jab;
using TypeCode.Wpf.Pages.String.Length;
using TypeCode.Wpf.Pages.String.Replace;

namespace TypeCode.Wpf.Pages.String;

[ServiceProviderModule]
[Transient(typeof(StringLengthView))]
[Transient(typeof(StringLengthViewModel))]
[Transient(typeof(StringReplaceView))]
[Transient(typeof(StringReplaceViewModel))]
[Transient(typeof(StringView))]
[Transient(typeof(StringViewModel))]
public interface IStringModule
{
    
}