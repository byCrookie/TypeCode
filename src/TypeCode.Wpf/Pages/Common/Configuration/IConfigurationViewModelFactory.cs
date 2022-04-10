namespace TypeCode.Wpf.Pages.Common.Configuration;

public interface IConfigurationViewModelFactory
{
    Task<ConfigurationWizardViewModel> CreateAsync();
}