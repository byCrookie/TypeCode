using Jab;
using TypeCode.Wpf.Helper.Event;
using TypeCode.Wpf.Helper.Navigation;
using TypeCode.Wpf.Helper.Thread;
using TypeCode.Wpf.Helper.ViewModels;

namespace TypeCode.Wpf.Helper;

[ServiceProviderModule]
[Import(typeof(INavigationModule))]
[Import(typeof(IEventModule))]
[Import(typeof(IViewModelModule))]
[Transient(typeof(IMinDelay), typeof(MinDelay))]
public interface IHelperModule
{
}