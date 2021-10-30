using System.Threading.Tasks;
using System.Windows.Input;
using AsyncAwaitBestPractices.MVVM;
using TypeCode.Wpf.Builder;
using TypeCode.Wpf.Composer;
using TypeCode.Wpf.Helper.Event;
using TypeCode.Wpf.Helper.Navigation;
using TypeCode.Wpf.Helper.ViewModel;
using TypeCode.Wpf.Mapper;
using TypeCode.Wpf.Specflow;
using TypeCode.Wpf.UnitTestDependencyManually;
using TypeCode.Wpf.UnitTestDependencyType;

namespace TypeCode.Wpf
{
	public class MainViewModel : Reactive, IAsyncEventHandler<AssemblyLoadedEvent>
	{
		private readonly INavigationService _navigationService;

		public MainViewModel(INavigationService navigationService, IEventAggregator eventAggregator)
		{
			_navigationService = navigationService;
			SpecflowNavigationCommand = new AsyncCommand(NavigateToSpecflowAsync);
			UnitTestDependencyTypeNavigationCommand = new AsyncCommand(NavigateToUnitTestDependencyTypeAsync);
			UnitTestDependencyManuallyNavigationCommand = new AsyncCommand(NavigateToUnitTestDependencyManuallyAsync);
			ComposerNavigationCommand = new AsyncCommand(NavigateToComposerAsync);
			MapperNavigationCommand = new AsyncCommand(NavigateToMapperAsync);
			BuilderNavigationCommand = new AsyncCommand(NavigateToBuilderAsync);

			ActiveItem = ActiveItem.None;
			AreAssembliesLoading = true;
			
			eventAggregator.Subscribe<AssemblyLoadedEvent>(this);
		}

		public ICommand SpecflowNavigationCommand { get; }
		public ICommand UnitTestDependencyTypeNavigationCommand { get; }
		public ICommand UnitTestDependencyManuallyNavigationCommand { get; }
		public ICommand ComposerNavigationCommand { get; }
		public ICommand MapperNavigationCommand { get; }
		public ICommand BuilderNavigationCommand { get; }
		
		public ActiveItem ActiveItem {
			get => Get<ActiveItem>();
			set => Set(value);
		}
		
		public bool AreAssembliesLoading {
			get => Get<bool>();
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
		
		private Task NavigateToComposerAsync()
		{
			ActiveItem = ActiveItem.Composer;
			return _navigationService.NavigateAsync<ComposerViewModel>();
		}
		
		private Task NavigateToMapperAsync()
		{
			ActiveItem = ActiveItem.Mapper;
			return _navigationService.NavigateAsync<MapperViewModel>();
		}
		
		private Task NavigateToBuilderAsync()
		{
			ActiveItem = ActiveItem.Builder;
			return _navigationService.NavigateAsync<BuilderViewModel>();
		}

		public Task HandleAsync(AssemblyLoadedEvent e)
		{
			AreAssembliesLoading = false;
			return Task.CompletedTask;
		}
	}
}