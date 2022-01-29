using Jab;
using TypeCode.Wpf.Pages.Common.Configuration;

namespace TypeCode.Wpf.Pages.Common;

[ServiceProviderModule]
[Transient(typeof(ConfigurationWizardView))]
[Transient(typeof(ConfigurationWizardViewModel))]
public partial interface ICommonWizardModule
{
}