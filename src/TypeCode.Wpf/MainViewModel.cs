using System.Threading.Tasks;
using System.Windows.Input;
using AsyncAwaitBestPractices.MVVM;
using TypeCode.Wpf.Helper.Navigation;
using TypeCode.Wpf.Helper.ViewModel;
using TypeCode.Wpf.Specflow;
using TypeCode.Wpf.UnitTestDependencyManually;
using TypeCode.Wpf.UnitTestDependencyType;

namespace TypeCode.Wpf
{
	public class MainViewModel : Reactive
	{
		private readonly INavigationService _navigationService;

		public MainViewModel(INavigationService navigationService)
		{
			_navigationService = navigationService;
			SpecflowNavigationCommand = new AsyncCommand(NavigateToSpecflowAsync);
			UnitTestDependencyTypeNavigationCommand = new AsyncCommand(NavigateToUnitTestDependencyTypeAsync);
			UnitTestDependencyManuallyNavigationCommand = new AsyncCommand(NavigateToUnitTestDependencyManuallyAsync);

			ActiveItem = ActiveItem.None;
		}

		public ICommand SpecflowNavigationCommand { get; }
		public ICommand UnitTestDependencyTypeNavigationCommand { get; }
		public ICommand UnitTestDependencyManuallyNavigationCommand { get; }
		
		public ActiveItem ActiveItem {
			get => Get<ActiveItem>();
			set => Set(value);
		}

		private Task NavigateToSpecflowAsync()
		{
			ActiveItem = ActiveItem.Specflow;
			return _navigationService.NavigateAsync<SpecflowViewModel>();
		}
		
		private Task NavigateToUnitTestDependencyTypeAsync()
		{
			ActiveItem = ActiveItem.UnitTestType;
			return _navigationService.NavigateAsync<UnitTestDependencyTypeViewModel>();
		}
		
		private Task NavigateToUnitTestDependencyManuallyAsync()
		{
			ActiveItem = ActiveItem.UnitTestManually;
			return _navigationService.NavigateAsync<UnitTestDependencyManuallyViewModel>();
		}
	}
}