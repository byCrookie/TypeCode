using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DependencyInjection.Factory;
using TypeCode.Wpf.Helper.Navigation.Contract;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.Navigation.Wizard;
using TypeCode.Wpf.Helper.ViewModels;
using TypeCode.Wpf.Pages.Assemblies;
using TypeCode.Wpf.Pages.Builder;
using TypeCode.Wpf.Pages.Common.Configuration;
using TypeCode.Wpf.Pages.Composer;
using TypeCode.Wpf.Pages.DynamicExecution;
using TypeCode.Wpf.Pages.Encoding;
using TypeCode.Wpf.Pages.Guid;
using TypeCode.Wpf.Pages.Home;
using TypeCode.Wpf.Pages.Mapper;
using TypeCode.Wpf.Pages.Specflow;
using TypeCode.Wpf.Pages.String;
using TypeCode.Wpf.Pages.UnitTest;

namespace TypeCode.Wpf.Main.Sidebar;

public partial class MainSidebarViewModel : ViewModelBase, IAsyncNavigatedTo
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

        ActiveItem = ActiveItem.None;
    }

    [ObservableProperty]
    private ActiveItem _activeItem;

    public Task OnNavigatedToAsync(NavigationContext context)
    {
        return _navigationService.NavigateAsync<HomeViewModel>(new NavigationContext());
    }

    [RelayCommand]
    private Task NavigateToSpecflowAsync()
    {
        ActiveItem = ActiveItem.Specflow;
        return _navigationService.NavigateAsync<SpecflowViewModel>(new NavigationContext());
    }

    [RelayCommand]
    private Task NavigateToUnitTestAsync()
    {
        ActiveItem = ActiveItem.UnitTest;
        return _navigationService.NavigateAsync<UnitTestViewModel>(new NavigationContext());
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
        return _navigationService.NavigateAsync<StringViewModel>(new NavigationContext());
    }

    [RelayCommand]
    private Task NavigateToGuidAsync()
    {
        ActiveItem = ActiveItem.Guid;
        return _navigationService.NavigateAsync<GuidViewModel>(new NavigationContext());
    }

    [RelayCommand]
    private Task NavigateToEncodingAsync()
    {
        ActiveItem = ActiveItem.Encoding;
        return _navigationService.NavigateAsync<EncodingViewModel>(new NavigationContext());
    }
    
    [RelayCommand]
    private Task NavigateToStringsAsync()
    {
        ActiveItem = ActiveItem.String;
        return _navigationService.NavigateAsync<StringViewModel>(new NavigationContext());
    }

    [RelayCommand]
    private Task OpenSettingsAsync()
    {
        var wizardBuilder = _wizardBuilderFactory.Create();

        var wizard = wizardBuilder
            .Then<SetupWizardViewModel>((options, _) => options.AllowNext(_ => true).Title("Configuration"))
            .FinishAsync(context => context.GetParameter<SetupConfigurator>().ExportAsync(), "Save")
            .Build();

        return _settingsWizardRunner.RunAsync(wizard);
    }
}