using System.Threading.Tasks;
using System.Windows;

namespace TypeCode.Wpf.Helper.Navigation.Wizard.Complex
{
    internal class WizardRunner : IWizardRunner
    {
        public Task RunAsync(Wizard wizard)
        {
            wizard.Content.Opacity = 0.1;
            wizard.Content.IsEnabled = false;
            wizard.WizardOverlay.Visibility = Visibility.Visible;
            
            wizard.NavigationFrame.Navigate(wizard.WizardInstances.ViewInstance);

            var wizardHost = (IWizardHost)wizard.WizardInstances.ViewModelInstance;
            wizardHost.NavigateToAsync(wizard);
            
            return Task.CompletedTask;
        }
    }
}