using TypeCode.Wpf.Helper.Navigation.Service;

namespace TypeCode.Wpf.Pages.Common.Configuration;

public class ConfigurationViewModelFactory : IConfigurationViewModelFactory
{
    public async Task<ConfigurationWizardViewModel> CreateAsync()
    {
        var viewModel = new ConfigurationWizardViewModel();
        await viewModel.OnInititalNavigationAsync(new NavigationContext()).ConfigureAwait(true);
        return viewModel;
    }
}