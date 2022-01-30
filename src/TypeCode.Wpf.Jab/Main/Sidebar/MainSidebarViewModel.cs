using System.Threading.Tasks;
using System.Windows.Input;
using AsyncAwaitBestPractices.MVVM;
using Framework.DependencyInjection.Factory;
using TypeCode.Wpf.Jab.Helper.Navigation.Service;
using TypeCode.Wpf.Jab.Helper.Navigation.Wizard.Complex;
using TypeCode.Wpf.Jab.Helper.ViewModel;
using TypeCode.Wpf.Jab.Pages.Assemblies;
using TypeCode.Wpf.Jab.Pages.Builder;
using TypeCode.Wpf.Jab.Pages.Common.Configuration;
using TypeCode.Wpf.Jab.Pages.Composer;
using TypeCode.Wpf.Jab.Pages.Mapper;
using TypeCode.Wpf.Jab.Pages.Specflow;
using TypeCode.Wpf.Jab.Pages.UnitTestDependencyManually;
using TypeCode.Wpf.Jab.Pages.UnitTestDependencyType;

namespace TypeCode.Wpf.Jab.Main.Sidebar;

public class MainSidebarViewModel : Reactive
{
    private readonly INavigationService _navigationService;
    private readonly IWizardBuilder _settingsWizardBuilder;
    private readonly IWizardRunner _settingsWizardRunner;
    private readonly IFactory _factory;

    public MainSidebarViewModel(
        INavigationService navigationService,
        IWizardBuilder settingsWizardBuilder,
        IWizardRunner settingsWizardRunner,
        IFactory factory
    )
    {
        _navigationService = navigationService;
        _settingsWizardBuilder = settingsWizardBuilder;
        _settingsWizardRunner = settingsWizardRunner;
        _factory = factory;

        SpecflowNavigationCommand = new AsyncCommand(NavigateToSpecflowAsync);
        UnitTestDependencyTypeNavigationCommand = new AsyncCommand(NavigateToUnitTestDependencyTypeAsync);
        UnitTestDependencyManuallyNavigationCommand = new AsyncCommand(NavigateToUnitTestDependencyManuallyAsync);
        ComposerNavigationCommand = new AsyncCommand(NavigateToComposerAsync);
        MapperNavigationCommand = new AsyncCommand(NavigateToMapperAsync);
        BuilderNavigationCommand = new AsyncCommand(NavigateToBuilderAsync);
        AssemblyNavigationCommand = new AsyncCommand(NavigateToAssemblyAsync);
        OpenSettingsCommand = new AsyncCommand(OpenSettingsAsync);

        ActiveItem = ActiveItem.None;
    }

    public ICommand SpecflowNavigationCommand { get; }
    public ICommand UnitTestDependencyTypeNavigationCommand { get; }
    public ICommand UnitTestDependencyManuallyNavigationCommand { get; }
    public ICommand ComposerNavigationCommand { get; }
    public ICommand MapperNavigationCommand { get; }
    public ICommand BuilderNavigationCommand { get; }
    public ICommand AssemblyNavigationCommand { get; }
    public ICommand OpenSettingsCommand { get; }

    public ActiveItem ActiveItem
    {
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

    private Task NavigateToAssemblyAsync()
    {
        ActiveItem = ActiveItem.Assembly;
        return _navigationService.NavigateAsync<AssemblyViewModel>();
    }

    private Task OpenSettingsAsync()
    {
        var mainWindow = _factory.Create<MainWindow>();
        var wizard = _settingsWizardBuilder
            .Init(new NavigationContext(), mainWindow.WizardFrame, mainWindow.Main, mainWindow.WizardOverlay)
            .Then<ConfigurationWizardViewModel>()
            .Build();
        return _settingsWizardRunner.RunAsync(wizard);
    }
}