using Jab;
using TypeCode.Wpf.Helper.Event;
using TypeCode.Wpf.Helper.Navigation;

namespace TypeCode.Wpf.Helper;

[ServiceProviderModule]
[Import(typeof(INavigationModule))]
[Import(typeof(IEventModule))]
public interface IHelperModule
{
}