using Jab;

namespace TypeCode.Wpf.Helper.Navigation.Wizard.Steps.WizardStep;

[ServiceProviderModule]
[Transient(typeof(IWizardStep<,,>), typeof(WizardStep<,,>))]
internal partial interface IWizardStepModule
{
}