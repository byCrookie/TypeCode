using System.Threading.Tasks;

namespace TypeCode.Wpf.Helper.Navigation.Service;

public interface INavigationService
{
	Task NavigateAsync<T>(NavigationContext context = null);
}