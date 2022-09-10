using Jab;
using TypeCode.Wpf.Pages.Configuration.Simple.AssemblyGroupWizard;
using TypeCode.Wpf.Pages.Configuration.Simple.AssemblyPathSelectorWizard;
using TypeCode.Wpf.Pages.Configuration.Simple.AssemblyPathWizard;
using TypeCode.Wpf.Pages.Configuration.Simple.AssemblyRootWizard;
using TypeCode.Wpf.Pages.Configuration.Simple.IncludeAssemblyPatternWizard;

namespace TypeCode.Wpf.Pages.Configuration.Simple;

[ServiceProviderModule]
[Transient(typeof(SimpleConfigurationWizardView))]
[Transient(typeof(SimpleConfigurationWizardViewModel))]
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
[Transient(typeof(ISimpleConfigurator), typeof(SimpleConfigurator))]
public interface ISimpleConfigurationModule
{
    
}