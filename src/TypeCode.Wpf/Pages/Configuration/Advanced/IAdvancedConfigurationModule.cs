using Jab;

namespace TypeCode.Wpf.Pages.Configuration.Advanced;

[ServiceProviderModule]
[Transient(typeof(IAdvancedConfigurationViewModelFactory), typeof(AdvancedConfigurationViewModelFactory))]
public interface IAdvancedConfigurationModule
{
    
}