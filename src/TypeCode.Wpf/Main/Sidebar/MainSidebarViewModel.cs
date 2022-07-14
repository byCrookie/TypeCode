using System.Windows.Input;
using DependencyInjection.Factory;
using TypeCode.Wpf.Helper.Commands;
using TypeCode.Wpf.Helper.Navigation.Contract;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.Navigation.Wizard.Complex;
using TypeCode.Wpf.Helper.ViewModel;
using TypeCode.Wpf.Pages.Assemblies;
using TypeCode.Wpf.Pages.Builder;
using TypeCode.Wpf.Pages.Common.Configuration;
using TypeCode.Wpf.Pages.Composer;
using TypeCode.Wpf.Pages.DynamicExecution;
using TypeCode.Wpf.Pages.Home;
using TypeCode.Wpf.Pages.Mapper;
using TypeCode.Wpf.Pages.Specflow;
using TypeCode.Wpf.Pages.UnitTestDependencyManually;
using TypeCode.Wpf.Pages.UnitTestDependencyType;

namespace TypeCode.Wpf.Main.Sidebar;

public class MainSidebarViewModel : Reactive, IAsyncNavigatedTo
{
    private readonly INavigationService _navigationService;
    private readonly IFactory<IWizardBuilder> _wizardBuilderFactory;
    private readonly IWizardRunner _settingsWizardRunner;

    public MainSidebarViewModel(
        INavigationService navigationService,
        IFactory<IWizardBuilder> wizardBuilderFactory,
        IWizardRunner settingsWizardRunner
    )
    {
        _navigationService = navigationService;
        _wizardBuilderFactory = wizardBuilderFactory;
        _settingsWizardRunner = settingsWizardRunner;

        HomeNavigationCommand = new AsyncRelayCommand(NavigateToHomeAsync);
        SpecflowNavigationCommand = new AsyncRelayCommand(NavigateToSpecflowAsync);
        UnitTestDependencyTypeNavigationCommand = new AsyncRelayCommand(NavigateToUnitTestDependencyTypeAsync);
        UnitTestDependencyManuallyNavigationCommand = new AsyncRelayCommand(NavigateToUnitTestDependencyManuallyAsync);
        ComposerNavigationCommand = new AsyncRelayCommand(NavigateToComposerAsync);
        MapperNavigationCommand = new AsyncRelayCommand(NavigateToMapperAsync);
        BuilderNavigationCommand = new AsyncRelayCommand(NavigateToBuilderAsync);
        AssemblyNavigationCommand = new AsyncRelayCommand(NavigateToAssemblyAsync);
        DynamicExecuteNavigationCommand = new AsyncRelayCommand(NavigateToDynamicExecuteAsync);
        OpenSettingsCommand = new AsyncRelayCommand(OpenSettingsAsync);

        ActiveItem = ActiveItem.Home;
    }

    public ICommand HomeNavigationCommand { get; }
    public ICommand SpecflowNavigationCommand { get; }
    public ICommand UnitTestDependencyTypeNavigationCommand { get; }
    public ICommand UnitTestDependencyManuallyNavigationCommand { get; }
    public ICommand ComposerNavigationCommand { get; }
    public ICommand MapperNavigationCommand { get; }
    public ICommand BuilderNavigationCommand { get; }
    public ICommand AssemblyNavigationCommand { get; }
    public ICommand DynamicExecuteNavigationCommand { get; }
    public ICommand OpenSettingsCommand { get; }

    public ActiveItem ActiveItem
    {
        get => Get<ActiveItem>();
        set => Set(value);
    }

    public Task OnNavigatedToAsync(NavigationContext context)
    {
        return NavigateToHomeAsync();
    }

    private Task NavigateToHomeAsync()
    {
        ActiveItem = ActiveItem.Home;
        return _navigationService.NavigateAsync<HomeViewModel>(new NavigationContext());
    }

    private Task NavigateToSpecflowAsync()
    {
        ActiveItem = ActiveItem.Specflow;
        return _navigationService.NavigateAsync<SpecflowViewModel>(new NavigationContext());
    }

    private Task NavigateToUnitTestDependencyTypeAsync()
    {
        ActiveItem = ActiveItem.UnitTestType;
        return _navigationService.NavigateAsync<UnitTestDependencyTypeViewModel>(new NavigationContext());
    }

    private Task NavigateToUnitTestDependencyManuallyAsync()
    {
        ActiveItem = ActiveItem.UnitTestManually;
        return _navigationService.NavigateAsync<UnitTestDependencyManuallyViewModel>(new NavigationContext());
    }

    private Task NavigateToComposerAsync()
    {
        ActiveItem = ActiveItem.Composer;
        return _navigationService.NavigateAsync<ComposerViewModel>(new NavigationContext());
    }

    private Task NavigateToMapperAsync()
    {
        ActiveItem = ActiveItem.Mapper;
        return _navigationService.NavigateAsync<MapperViewModel>(new NavigationContext());
    }

    private Task NavigateToBuilderAsync()
    {
        ActiveItem = ActiveItem.Builder;
        return _navigationService.NavigateAsync<BuilderViewModel>(new NavigationContext());
    }

    private Task NavigateToAssemblyAsync()
    {
        ActiveItem = ActiveItem.Assembly;
        return _navigationService.NavigateAsync<AssemblyViewModel>(new NavigationContext());
    }

    private Task NavigateToDynamicExecuteAsync()
    {
        ActiveItem = ActiveItem.DynamicExecute;
        return _navigationService.NavigateAsync<DynamicExecutionViewModel>(new NavigationContext());
    }

    private Task OpenSettingsAsync()
    {
        var wizardBuilder = _wizardBuilderFactory.Create();

        var wizard = wizardBuilder
            .Then<SetupWizardViewModel>((options, _) => options.AllowNext(_ => true))
            .FinishAsync(context => context.GetParameter<SetupConfigurator>().ExportAsync(), "Save")
            .Build();

        return _settingsWizardRunner.RunAsync(wizard);
    }
}