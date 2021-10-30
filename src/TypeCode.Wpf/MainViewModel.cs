using System.Threading.Tasks;
using System.Windows.Input;
using TypeCode.Wpf.Helper.Commands;
using TypeCode.Wpf.Helper.Navigation;
using TypeCode.Wpf.Helper.ViewModel;
using TypeCode.Wpf.Specflow;

namespace TypeCode.Wpf
{
	public class MainViewModel : Reactive
	{
		private readonly INavigationService _navigationService;

		public MainViewModel(INavigationService navigationService)
		{
			_navigationService = navigationService;
			SpecflowNavigationCommand = new AsyncRelayCommand(NavigateToSpecflowAsync);

			ActiveItem = ActiveItem.None;
		}

		public ICommand SpecflowNavigationCommand { get; }
		
		public ActiveItem ActiveItem {
			get => Get<ActiveItem>();
			set => Set(value);
		}

		private Task NavigateToSpecflowAsync(object parameter)
		{
			ActiveItem = ActiveItem.Specflow;
			return _navigationService.NavigateAsync<SpecflowViewModel>();
		}
	}
}