using TypeCode.Business.Bootstrapping.Data;
using TypeCode.Wpf.Helper.Navigation.Service;

namespace TypeCode.Wpf.Pages.Common.Configuration;

public class ConfigurationViewModelFactory : IConfigurationViewModelFactory
{
    private readonly IUserDataLocationProvider _userDataLocationProvider;

    public ConfigurationViewModelFactory(IUserDataLocationProvider userDataLocationProvider)
    {
        _userDataLocationProvider = userDataLocationProvider;
    }
    
    public async Task<ConfigurationWizardViewModel> CreateAsync()
    {
        var viewModel = new ConfigurationWizardViewModel(_userDataLocationProvider);
        await viewModel.OnInititalNavigationAsync(new NavigationContext()).ConfigureAwait(true);
        return viewModel;
    }
}