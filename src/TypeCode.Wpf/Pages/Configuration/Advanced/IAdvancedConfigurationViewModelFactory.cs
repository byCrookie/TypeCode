namespace TypeCode.Wpf.Pages.Configuration.Advanced;

public interface IAdvancedConfigurationViewModelFactory
{
    Task<AdvancedConfigurationWizardViewModel> CreateAsync();
}