using TypeCode.Business.Bootstrapping.Data;
using TypeCode.Wpf.Helper.Navigation.Service;

namespace TypeCode.Wpf.Pages.Configuration.Advanced;

public class AdvancedConfigurationViewModelFactory : IAdvancedConfigurationViewModelFactory
{
    private readonly IUserDataLocationProvider _userDataLocationProvider;

    public AdvancedConfigurationViewModelFactory(IUserDataLocationProvider userDataLocationProvider)
    {
        _userDataLocationProvider = userDataLocationProvider;
    }
    
    public async Task<AdvancedConfigurationWizardViewModel> CreateAsync()
    {
        var viewModel = new AdvancedConfigurationWizardViewModel(_userDataLocationProvider);
        await viewModel.OnInititalNavigationAsync(new NavigationContext()).ConfigureAwait(true);
        return viewModel;
    }
}