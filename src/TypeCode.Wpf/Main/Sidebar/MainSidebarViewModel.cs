using System.Threading.Tasks;
using System.Windows.Input;
using AsyncAwaitBestPractices.MVVM;
using TypeCode.Wpf.Helper.Navigation;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.ViewModel;
using TypeCode.Wpf.Pages.Builder;
using TypeCode.Wpf.Pages.Composer;
using TypeCode.Wpf.Pages.Mapper;
using TypeCode.Wpf.Pages.Specflow;
using TypeCode.Wpf.Pages.UnitTestDependencyManually;
using TypeCode.Wpf.Pages.UnitTestDependencyType;

namespace TypeCode.Wpf.Main.Sidebar
{
    public class MainSidebarViewModel : Reactive
    {
        private readonly INavigationService _navigationService;

        public MainSidebarViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            
            SpecflowNavigationCommand = new AsyncCommand(NavigateToSpecflowAsync);
            UnitTestDependencyTypeNavigationCommand = new AsyncCommand(NavigateToUnitTestDependencyTypeAsync);
            UnitTestDependencyManuallyNavigationCommand = new AsyncCommand(NavigateToUnitTestDependencyManuallyAsync);
            ComposerNavigationCommand = new AsyncCommand(NavigateToComposerAsync);
            MapperNavigationCommand = new AsyncCommand(NavigateToMapperAsync);
            BuilderNavigationCommand = new AsyncCommand(NavigateToBuilderAsync);

            ActiveItem = ActiveItem.None;
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
    }
}