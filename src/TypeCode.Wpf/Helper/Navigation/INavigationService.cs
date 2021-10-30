using System.Threading.Tasks;
using TypeCode.Wpf.Helper.ViewModel;

namespace TypeCode.Wpf.Helper.Navigation
{
	public interface INavigationService
	{
		Task NavigateAsync<T>(object parameter)  where T : ViewModelBase;
	}
}
