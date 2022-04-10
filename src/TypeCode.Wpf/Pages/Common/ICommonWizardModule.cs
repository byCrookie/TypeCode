using Jab;
using TypeCode.Wpf.Pages.Common.Configuration;
using TypeCode.Wpf.Pages.Common.Configuration.AssemblyGroupWizard;
using TypeCode.Wpf.Pages.Common.Configuration.AssemblyPathSelectorWizard;
using TypeCode.Wpf.Pages.Common.Configuration.AssemblyPathWizard;
using TypeCode.Wpf.Pages.Common.Configuration.AssemblyRootWizard;
using TypeCode.Wpf.Pages.Common.Configuration.IncludeAssemblyPatternWizard;

namespace TypeCode.Wpf.Pages.Common;

[ServiceProviderModule]
[Transient(typeof(SetupWizardView))]
[Transient(typeof(SetupWizardViewModel))]
[Transient(typeof(AssemblyRootWizardView))]
[Transient(typeof(AssemblyRootWizardViewModel))]
[Transient(typeof(AssemblyGroupWizardView))]
[Transient(typeof(AssemblyGroupWizardViewModel))]
[Transient(typeof(AssemblyPathWizardView))]
[Transient(typeof(AssemblyPathWizardViewModel))]
[Transient(typeof(AssemblyPathSelectorWizardView))]
[Transient(typeof(AssemblyPathSelectorWizardViewModel))]
[Transient(typeof(IncludeAssemblyPatternWizardView))]
[Transient(typeof(IncludeAssemblyPatternWizardViewModel))]
[Transient(typeof(ISetupConfigurator), typeof(SetupConfigurator))]
[Transient(typeof(IConfigurationViewModelFactory), typeof(ConfigurationViewModelFactory))]
public interface ICommonWizardModule
{
}