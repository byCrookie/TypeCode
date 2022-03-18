using Jab;
using TypeCode.Wpf.Pages.Common.Configuration;
using TypeCode.Wpf.Pages.Common.Configuration.AssemblyRoot;

namespace TypeCode.Wpf.Pages.Common;

[ServiceProviderModule]
[Transient(typeof(ConfigurationWizardView))]
[Transient(typeof(ConfigurationWizardViewModel))]
[Transient(typeof(SetupWizardView))]
[Transient(typeof(SetupWizardViewModel))]
[Transient(typeof(AssemblyRootWizardView))]
[Transient(typeof(AssemblyRootWizardViewModel))]
public interface ICommonWizardModule
{
}