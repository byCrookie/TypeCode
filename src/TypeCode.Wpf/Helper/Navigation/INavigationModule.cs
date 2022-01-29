using Jab;
using TypeCode.Wpf.Helper.Navigation.Modal;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.Navigation.Wizard;
using TypeCode.Wpf.Helper.Navigation.Wizard.Complex;

namespace TypeCode.Wpf.Helper.Navigation;

[ServiceProviderModule]
[Singleton(typeof(INavigationService), typeof(NavigationService))]
[Import(typeof(IModalModule))]
[Import(typeof(IWizardModule))]
[Import(typeof(IWizardComplexModule))]
public partial interface INavigationModule
{
}