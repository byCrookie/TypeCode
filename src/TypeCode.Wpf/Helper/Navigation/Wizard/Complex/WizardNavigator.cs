using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace TypeCode.Wpf.Helper.Navigation.Wizard.Complex
{
    public class WizardNavigator : IWizardNavigator
    {
        public async Task Next(Wizard wizard)
        {
            var wizardHost = (IWizardHost)wizard.WizardInstances.ViewModelInstance;
            await wizardHost.NavigateFromAsync(wizard).ConfigureAwait(true);

            var currentStepConfigurationIndex = wizard.StepConfigurations.IndexOf(wizard.CurrentStepConfiguration);
            wizard.CurrentStepConfiguration = wizard.StepConfigurations[currentStepConfigurationIndex + 1];
            
            await wizardHost.NavigateToAsync(wizard).ConfigureAwait(true);
        }

        public async Task Back(Wizard wizard)
        {
            if (wizard.CurrentStepConfiguration == wizard.StepConfigurations.First())
            {
                return;
            }
            
            var wizardHost = (IWizardHost)wizard.WizardInstances.ViewModelInstance;
            await wizardHost.NavigateFromAsync(wizard).ConfigureAwait(true);
            
            var currentStepConfigurationIndex = wizard.StepConfigurations.IndexOf(wizard.CurrentStepConfiguration);
            wizard.CurrentStepConfiguration = wizard.StepConfigurations[currentStepConfigurationIndex - 1];
            
            await wizardHost.NavigateToAsync(wizard).ConfigureAwait(true);
        }

        public Task Cancel(Wizard wizard)
        {
            wizard.Content.Opacity = 1;
            wizard.Content.IsEnabled = true;
            wizard.WizardOverlay.Visibility = Visibility.Collapsed;
            return Task.CompletedTask;
        }

        public async Task Finish(Wizard wizard)
        {
            await wizard.CompletedAction(wizard.NavigationContext).ConfigureAwait(true);
            
            wizard.Content.Opacity = 1;
            wizard.Content.IsEnabled = true;
            wizard.WizardOverlay.Visibility = Visibility.Collapsed;
        }
    }
}