using Jab;
using TypeCode.Wpf.Helper.Event;
using TypeCode.Wpf.Helper.Navigation;
using TypeCode.Wpf.Helper.Thread;

namespace TypeCode.Wpf.Helper;

[ServiceProviderModule]
[Import(typeof(INavigationModule))]
[Import(typeof(IEventModule))]
[Transient(typeof(IMinDelay), typeof(MinDelay))]
public interface IHelperModule
{
}