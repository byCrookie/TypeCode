using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.Navigation.Wizard.WizardSimple;

namespace TypeCode.Wpf.Pages.TypeSelection;

internal class TypeSelectionWizardStarter : ITypeSelectionWizardStarter
{
    private readonly IWizardNavigationService _wizardNavigationService;

    public TypeSelectionWizardStarter(IWizardNavigationService wizardNavigationService)
    {
        _wizardNavigationService = wizardNavigationService;
    }

    public Task StartAsync(TypeSelectionParameter parameter, Func<IEnumerable<Type>, Task> onSaveAction, Func<IEnumerable<Type>, Task> onCancelAction)
    {
        var navigationContext = new NavigationContext();
        navigationContext.AddParameter(parameter);

        return _wizardNavigationService
            .OpenWizardAsync(new WizardParameter<TypeSelectionViewModel>
            {
                FinishButtonText = "Select",
                CanSave = vm => vm.SelectedTypes.Any(),
                OnCancelAsync = (vm, _) => onCancelAction(vm.SelectedTypes),
                OnSaveAsync = (vm, _) => onSaveAction(vm.SelectedTypes)
            }, navigationContext);
    }
}