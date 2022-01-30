using System.Threading.Tasks;

namespace TypeCode.Wpf.Jab.Helper.Navigation.Service;

public interface INavigationService
{
	Task NavigateAsync<T>(NavigationContext context = null);
}