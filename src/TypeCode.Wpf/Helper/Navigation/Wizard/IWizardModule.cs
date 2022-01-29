using Jab;
using TypeCode.Wpf.Helper.Navigation.Wizard.Steps;
using TypeCode.Wpf.Helper.Navigation.Wizard.View;
using TypeCode.Wpf.Helper.Navigation.Wizard.WizardSimple;
using WizardSimpleView = TypeCode.Wpf.Helper.Navigation.Wizard.WizardSimple.WizardSimpleView;
using WizardView = TypeCode.Wpf.Helper.Navigation.Wizard.Complex.WizardView;

namespace TypeCode.Wpf.Helper.Navigation.Wizard;

[ServiceProviderModule]
[Import(typeof(IStepsModule))]
[Transient(typeof(IWizardNavigator), typeof(WizardNavigator))]
[Transient(typeof(WizardView))]
[Transient(typeof(WizardSimpleViewModel))]
[Transient(typeof(IWizardNavigationService), typeof(WizardNavigationService))]
[Transient(typeof(WizardSimpleView))]
[Transient(typeof(WizardSimpleViewModel<>))]
public partial interface IWizardModule
{
}