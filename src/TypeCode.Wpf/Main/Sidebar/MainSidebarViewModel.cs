using System.Windows.Input;
using AsyncAwaitBestPractices.MVVM;
using Framework.DependencyInjection.Factory;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.Navigation.Wizard.Complex;
using TypeCode.Wpf.Helper.ViewModel;
using TypeCode.Wpf.Pages.Assemblies;
using TypeCode.Wpf.Pages.Builder;
using TypeCode.Wpf.Pages.Common.Configuration;
using TypeCode.Wpf.Pages.Composer;
using TypeCode.Wpf.Pages.Mapper;
using TypeCode.Wpf.Pages.Specflow;
using TypeCode.Wpf.Pages.UnitTestDependencyManually;
using TypeCode.Wpf.Pages.UnitTestDependencyType;

namespace TypeCode.Wpf.Main.Sidebar;

public class MainSidebarViewModel : Reactive
{
    private readonly INavigationService _navigationService;
    private readonly IFactory<NavigationContext, IWizardBuilder> _wizardBuilderFactory;
    private readonly IWizardRunner _settingsWizardRunner;

    public MainSidebarViewModel(
        INavigationService navigationService,
        IFactory<NavigationContext, IWizardBuilder> wizardBuilderFactory,
        IWizardRunner settingsWizardRunner
    )
    {
        _navigationService = navigationService;
        _wizardBuilderFactory = wizardBuilderFactory;
        _settingsWizardRunner = settingsWizardRunner;

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

    private Task OpenSettingsAsync()
    {
        var wizardBuilder = _wizardBuilderFactory.Create(new NavigationContext());

        var wizard = wizardBuilder
            .Then<SetupWizardViewModel>()
            .Then<ConfigurationWizardViewModel>()
            .Build();
        
        return _settingsWizardRunner.RunAsync(wizard);
    }
}