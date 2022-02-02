using System.Windows;
using TypeCode.Wpf.Helper.Navigation.Service;

namespace TypeCode.Wpf.Helper.Navigation.Wizard.Complex;

internal class WizardRunner : IWizardRunner
{
    public async Task<NavigationContext> RunAsync(Wizard wizard)
    {
        wizard.Content.Opacity = 0.1;
        wizard.Content.IsEnabled = false;
        wizard.WizardOverlay.Visibility = Visibility.Visible;
            
        wizard.NavigationFrame.Navigate(wizard.WizardInstances.ViewInstance);

        var wizardHost = (IWizardHost)wizard.WizardInstances.ViewModelInstance;
        await wizardHost.NavigateToAsync(wizard, NavigationAction.Next).ConfigureAwait(true);
            
        return wizard.NavigationContext;
    }
}