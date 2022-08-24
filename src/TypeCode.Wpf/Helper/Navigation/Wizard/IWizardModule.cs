using Jab;

namespace TypeCode.Wpf.Helper.Navigation.Wizard;

[ServiceProviderModule]
[Transient(typeof(WizardView))]
[Transient(typeof(WizardViewModel))]
[Transient(typeof(IWizardBuilder), typeof(WizardBuilder))]
[Transient(typeof(IWizardParameterBuilder), typeof(WizardParameterBuilder))]
[Transient(typeof(IWizardNavigator), typeof(WizardNavigator))]
[Transient(typeof(IWizardRunner), typeof(WizardRunner))]
public interface IWizardModule
{
}