using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DependencyInjection.Factory;
using TypeCode.Wpf.Helper.Navigation.Contract;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.Navigation.Wizard;
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

public partial class MainSidebarViewModel : ObservableObject, IAsyncNavigatedTo
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

        ActiveItem = ActiveItem.Home;
    }

    [ObservableProperty]
    private ActiveItem _activeItem;

    public Task OnNavigatedToAsync(NavigationContext context)
    {
        return NavigateToHomeAsync();
    }

    [RelayCommand]
    private Task NavigateToHomeAsync()
    {
        ActiveItem = ActiveItem.Home;
        return _navigationService.NavigateAsync<HomeViewModel>(new NavigationContext());
    }

    [RelayCommand]
    private Task NavigateToSpecflowAsync()
    {
        ActiveItem = ActiveItem.Specflow;
        return _navigationService.NavigateAsync<SpecflowViewModel>(new NavigationContext());
    }

    [RelayCommand]
    private Task NavigateToUnitTestDependencyTypeAsync()
    {
        ActiveItem = ActiveItem.UnitTestType;
        return _navigationService.NavigateAsync<UnitTestDependencyTypeViewModel>(new NavigationContext());
    }

    [RelayCommand]
    private Task NavigateToUnitTestDependencyManuallyAsync()
    {
        ActiveItem = ActiveItem.UnitTestManually;
        return _navigationService.NavigateAsync<UnitTestDependencyManuallyViewModel>(new NavigationContext());
    }

    [RelayCommand]
    private Task NavigateToComposerAsync()
    {
        ActiveItem = ActiveItem.Composer;
        return _navigationService.NavigateAsync<ComposerViewModel>(new NavigationContext());
    }

    [RelayCommand]
    private Task NavigateToMapperAsync()
    {
        ActiveItem = ActiveItem.Mapper;
        return _navigationService.NavigateAsync<MapperViewModel>(new NavigationContext());
    }

    [RelayCommand]
    private Task NavigateToBuilderAsync()
    {
        ActiveItem = ActiveItem.Builder;
        return _navigationService.NavigateAsync<BuilderViewModel>(new NavigationContext());
    }

    [RelayCommand]
    private Task NavigateToAssemblyAsync()
    {
        ActiveItem = ActiveItem.Assembly;
        return _navigationService.NavigateAsync<AssemblyViewModel>(new NavigationContext());
    }

    [RelayCommand]
    private Task NavigateToDynamicExecuteAsync()
    {
        ActiveItem = ActiveItem.DynamicExecute;
        return _navigationService.NavigateAsync<DynamicExecutionViewModel>(new NavigationContext());
    }

    [RelayCommand]
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