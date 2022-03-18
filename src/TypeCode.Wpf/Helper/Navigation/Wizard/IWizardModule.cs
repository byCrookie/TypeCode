using Jab;
using TypeCode.Wpf.Helper.Navigation.Wizard.WizardSimple;

namespace TypeCode.Wpf.Helper.Navigation.Wizard;

[ServiceProviderModule]
[Transient(typeof(IWizardNavigationService), typeof(WizardNavigationService))]
[Transient(typeof(WizardSimpleViewModel<>))]
[Transient(typeof(WizardSimpleView))]
public interface IWizardModule
{
}