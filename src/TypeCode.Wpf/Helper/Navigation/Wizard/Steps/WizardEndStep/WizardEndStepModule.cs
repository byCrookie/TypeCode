using Jab;

namespace TypeCode.Wpf.Helper.Navigation.Wizard.Steps.WizardEndStep;

[ServiceProviderModule]
[Transient(typeof(IWizardEndStep<,>),typeof(WizardEndStep<,>))]
internal partial interface IWizardEndStepModule
{
}