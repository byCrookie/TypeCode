using Jab;
using TypeCode.Wpf.Pages.String.Length;
using TypeCode.Wpf.Pages.String.Replace;

namespace TypeCode.Wpf.Pages.String;

[ServiceProviderModule]
[Singleton(typeof(StringLengthView))]
[Singleton(typeof(StringLengthViewModel))]
[Singleton(typeof(StringReplaceView))]
[Singleton(typeof(StringReplaceViewModel))]
[Singleton(typeof(StringView))]
[Singleton(typeof(StringViewModel))]
public interface IStringModule
{
    
}