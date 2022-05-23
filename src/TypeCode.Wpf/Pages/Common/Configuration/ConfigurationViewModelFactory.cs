using TypeCode.Business.Configuration.Location;
using TypeCode.Wpf.Helper.Navigation.Service;

namespace TypeCode.Wpf.Pages.Common.Configuration;

public class ConfigurationViewModelFactory : IConfigurationViewModelFactory
{
    private readonly IConfigurationLocationProvider _configurationLocationProvider;

    public ConfigurationViewModelFactory(IConfigurationLocationProvider configurationLocationProvider)
    {
        _configurationLocationProvider = configurationLocationProvider;
    }
    
    public async Task<ConfigurationWizardViewModel> CreateAsync()
    {
        var viewModel = new ConfigurationWizardViewModel(_configurationLocationProvider);
        await viewModel.OnInititalNavigationAsync(new NavigationContext()).ConfigureAwait(true);
        return viewModel;
    }
}