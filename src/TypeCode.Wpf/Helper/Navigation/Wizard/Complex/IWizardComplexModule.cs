using Jab;

namespace TypeCode.Wpf.Helper.Navigation.Wizard.Complex;

[ServiceProviderModule]
[Transient(typeof(WizardView))]
[Transient(typeof(WizardViewModel))]
[Transient(typeof(IWizardBuilder), typeof(WizardBuilder))]
[Transient(typeof(IWizardNavigator), typeof(WizardNavigator))]
[Transient(typeof(IWizardRunner), typeof(WizardRunner))]
public partial interface IWizardComplexModule
{
}