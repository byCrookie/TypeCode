using TypeCode.Wpf.Helper.Event;

namespace TypeCode.Wpf.Helper.Navigation.Wizard.WizardSimple;

internal class WizardSimpleViewModelDataContext : WizardSimpleViewModel<object>
{
    public WizardSimpleViewModelDataContext(IWizardNavigationService wizardNavigationService)
        : base(wizardNavigationService, new EventAggregator())
    {
    }
}