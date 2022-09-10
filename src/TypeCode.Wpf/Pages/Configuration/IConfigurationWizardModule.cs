using Jab;
using TypeCode.Wpf.Pages.Configuration.Advanced;
using TypeCode.Wpf.Pages.Configuration.Simple;

namespace TypeCode.Wpf.Pages.Configuration;

[ServiceProviderModule]
[Import(typeof(IAdvancedConfigurationModule))]
[Import(typeof(ISimpleConfigurationModule))]
public interface IConfigurationWizardModule
{
}