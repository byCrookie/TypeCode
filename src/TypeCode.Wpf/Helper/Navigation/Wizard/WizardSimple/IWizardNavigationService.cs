using System.Threading.Tasks;
using TypeCode.Wpf.Helper.Navigation.Service;

namespace TypeCode.Wpf.Helper.Navigation.Wizard.WizardSimple;

public interface IWizardNavigationService
{
	Task<T> OpenWizardAsync<T>(WizardParameter<T> parameter, NavigationContext context = null);
	Task CloseWizardAsync<T>();
}