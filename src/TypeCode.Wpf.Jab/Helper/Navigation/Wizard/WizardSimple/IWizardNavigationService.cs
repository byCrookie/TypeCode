using System.Threading.Tasks;
using TypeCode.Wpf.Jab.Helper.Navigation.Service;

namespace TypeCode.Wpf.Jab.Helper.Navigation.Wizard.WizardSimple;

public interface IWizardNavigationService
{
	Task<T> OpenWizardAsync<T>(WizardParameter<T> parameter, NavigationContext context = null);
	Task CloseWizardAsync<T>();
}