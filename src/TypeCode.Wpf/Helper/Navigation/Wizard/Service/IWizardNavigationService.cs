using System.Threading.Tasks;
using TypeCode.Wpf.Helper.Navigation.Service;

namespace TypeCode.Wpf.Helper.Navigation.Wizard.Service
{
	public interface IWizardNavigationService
	{
		Task<T> OpenWizard<T>(WizardParameter<T> parameter, NavigationContext context = null);
		Task CloseWizard<T>();
	}
}
