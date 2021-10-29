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
		private string _testProperty;

		public MainViewModel(INavigationService navigationService)
		{
			_navigationService = navigationService;
			SpecflowNavigationCommand = new RelayCommand(NavigateToSpecflow);
		}

		public ICommand SpecflowNavigationCommand { get; set; }

		public string TestProperty
		{
			get => _testProperty;
			set
			{
				_testProperty = value;
				OnPropertyChanged();
			}
		}
		
		private void NavigateToSpecflow(object parameter)
		{
			_navigationService.Navigate<SpecflowViewModel>(null);
		}
	}
}