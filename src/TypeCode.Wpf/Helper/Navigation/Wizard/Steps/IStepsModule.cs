using Jab;
using TypeCode.Wpf.Helper.Navigation.Wizard.Steps.WizardEndStep;
using TypeCode.Wpf.Helper.Navigation.Wizard.Steps.WizardStep;

namespace TypeCode.Wpf.Helper.Navigation.Wizard.Steps;

[ServiceProviderModule]
[Import(typeof(IWizardStepModule))]
[Import(typeof(IWizardEndStepModule))]
public partial interface IStepsModule
{
}