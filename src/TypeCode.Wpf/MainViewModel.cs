using System.Threading.Tasks;
using System.Windows.Input;
using TypeCode.Wpf.Helper.Commands;
using TypeCode.Wpf.Helper.Navigation;
using TypeCode.Wpf.Helper.ViewModel;
using TypeCode.Wpf.Specflow;

namespace TypeCode.Wpf
{
	public class MainViewModel : ViewModelBase
	{
		private readonly INavigationService _navigationService;

		public MainViewModel(INavigationService navigationService)
		{
			_navigationService = navigationService;
			SpecflowNavigationCommand = new AsyncRelayCommand(NavigateToSpecflowAsync);
		}

		public ICommand SpecflowNavigationCommand { get; }

		private Task NavigateToSpecflowAsync(object parameter)
		{
			return _navigationService.NavigateAsync<SpecflowViewModel>(null);
		}
	}
}