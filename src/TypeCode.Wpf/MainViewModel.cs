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
		private ActiveItem _activeItem;

		public MainViewModel(INavigationService navigationService)
		{
			_navigationService = navigationService;
			SpecflowNavigationCommand = new AsyncRelayCommand(NavigateToSpecflowAsync);

			ActiveItem = ActiveItem.None;
		}

		public ICommand SpecflowNavigationCommand { get; }

		public ActiveItem ActiveItem
		{
			get => _activeItem;
			set
			{
				_activeItem = value;
				OnPropertyChanged();
			}
		}

		private Task NavigateToSpecflowAsync(object parameter)
		{
			ActiveItem = ActiveItem.Specflow;
			return _navigationService.NavigateAsync<SpecflowViewModel>(null);
		}
	}
}