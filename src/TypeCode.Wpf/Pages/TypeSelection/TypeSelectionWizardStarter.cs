using DependencyInjection.Factory;
using TypeCode.Wpf.Helper.Navigation.Wizard;

namespace TypeCode.Wpf.Pages.TypeSelection;

public sealed class TypeSelectionWizardStarter : ITypeSelectionWizardStarter
{
    private readonly IFactory<IWizardBuilder> _wizardBuilderFactory;
    private readonly IWizardRunner _wizardRunner;

    public TypeSelectionWizardStarter(
        IFactory<IWizardBuilder> wizardBuilderFactory,
        IWizardRunner wizardRunner
    )
    {
        _wizardBuilderFactory = wizardBuilderFactory;
        _wizardRunner = wizardRunner;
    }

    public Task StartAsync(TypeSelectionParameter parameter, Func<TypeSelectionViewModel, Task> onSaveAction)
    {
        var wizard = _wizardBuilderFactory.Create()
            .Then<TypeSelectionViewModel>((options, _) => options
                .Title(parameter.AllowMultiSelection ? "Select Types" : "Select Type")
                .AllowNext(c => c.GetParameter<TypeSelectionViewModel>().SelectedTypes.Any()))
            .FinishAsync(c => onSaveAction(c.GetParameter<TypeSelectionViewModel>()), "Select")
            .NavigationContext(c => c.AddParameter(parameter))
            .Build();

        return _wizardRunner.RunAsync(wizard);
    }
}