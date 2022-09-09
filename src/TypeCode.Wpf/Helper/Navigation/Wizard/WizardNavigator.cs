using System.Windows;
using TypeCode.Wpf.Helper.Event;

namespace TypeCode.Wpf.Helper.Navigation.Wizard;

public class WizardNavigator : IWizardNavigator
{
    private readonly IEventAggregator _eventAggregator;
    private readonly IWizardRunner _wizardRunner;

    public WizardNavigator(IEventAggregator eventAggregator, IWizardRunner wizardRunner)
    {
        _eventAggregator = eventAggregator;
        _wizardRunner = wizardRunner;
    }

    public async Task NextAsync(Wizard wizard)
    {
        var wizardHost = (IWizardHost)wizard.WizardInstances.ViewModelInstance;
        await wizardHost.NavigateFromAsync(wizard, NavigationAction.Next).ConfigureAwait(true);

        var currentStepConfigurationIndex = wizard.CurrentStepConfiguration is null ? -1 : wizard.StepConfigurations.IndexOf(wizard.CurrentStepConfiguration);
        wizard.CurrentStepConfiguration = wizard.StepConfigurations[currentStepConfigurationIndex + 1];

        await wizardHost.NavigateToAsync(wizard, NavigationAction.Next).ConfigureAwait(true);
    }

    public async Task BackAsync(Wizard wizard)
    {
        if (wizard.CurrentStepConfiguration == wizard.StepConfigurations.First())
        {
            return;
        }

        var wizardHost = (IWizardHost)wizard.WizardInstances.ViewModelInstance;
        await wizardHost.NavigateFromAsync(wizard, NavigationAction.Back).ConfigureAwait(true);

        var currentStepConfigurationIndex = wizard.CurrentStepConfiguration is null ? 1 : wizard.StepConfigurations.IndexOf(wizard.CurrentStepConfiguration);
        wizard.CurrentStepConfiguration = wizard.StepConfigurations[currentStepConfigurationIndex - 1];

        await wizardHost.NavigateToAsync(wizard, NavigationAction.Back).ConfigureAwait(true);
    }

    public Task CancelAsync(Wizard wizard)
    {
        wizard.Content.Opacity = 1;
        wizard.Content.IsEnabled = true;
        wizard.WizardOverlay.Visibility = Visibility.Collapsed;

        return wizard.NavigationContext.ContainsParameter(WizardConstants.LastWizard)
            ? _wizardRunner.RunAsync(wizard.NavigationContext.GetParameter<Wizard>(WizardConstants.LastWizard))
            : Task.CompletedTask;
    }

    public async Task FinishAsync(Wizard wizard)
    {
        await wizard.CompletedAction(wizard.NavigationContext).ConfigureAwait(true);

        if (wizard.CompletedEvent is not null)
        {
            await _eventAggregator.PublishAsync(wizard.CompletedEvent).ConfigureAwait(true);
        }

        wizard.Content.Opacity = 1;
        wizard.Content.IsEnabled = true;
        wizard.WizardOverlay.Visibility = Visibility.Collapsed;

        if (wizard.NavigationContext.ContainsParameter(WizardConstants.LastWizard))
        {
            var returnWizard = wizard.NavigationContext.GetParameter<Wizard>(WizardConstants.LastWizard);
            await _wizardRunner.RunAsync(returnWizard).ConfigureAwait(true);
        }
    }
}