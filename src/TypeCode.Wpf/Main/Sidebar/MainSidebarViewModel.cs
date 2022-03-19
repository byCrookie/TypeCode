using System.Windows.Input;
using AsyncAwaitBestPractices.MVVM;
using Framework.DependencyInjection.Factory;
using Serilog;
using TypeCode.Business.Configuration;
using TypeCode.Business.TypeEvaluation;
using TypeCode.Wpf.Application;
using TypeCode.Wpf.Helper.Event;
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

public class MainSidebarViewModel : Reactive, IAsyncEventHandler<LoadEndEvent>
{
    private readonly INavigationService _navigationService;
    private readonly IFactory _factory;
    private readonly IWizardRunner _settingsWizardRunner;
    private readonly IEventAggregator _eventAggregator;
    private readonly IConfigurationProvider _configurationProvider;
    private readonly IConfigurationLoader _configurationLoader;
    private readonly ITypeEvaluator _typeEvaluator;
    private readonly ITypeProvider _typeProvider;

    public MainSidebarViewModel(
        INavigationService navigationService,
        IFactory factory,
        IWizardRunner settingsWizardRunner,
        IEventAggregator eventAggregator,
        IConfigurationProvider configurationProvider,
        IConfigurationLoader configurationLoader,
        ITypeEvaluator typeEvaluator,
        ITypeProvider typeProvider
    )
    {
        _navigationService = navigationService;
        _factory = factory;
        _settingsWizardRunner = settingsWizardRunner;
        _eventAggregator = eventAggregator;
        _configurationProvider = configurationProvider;
        _configurationLoader = configurationLoader;
        _typeEvaluator = typeEvaluator;
        _typeProvider = typeProvider;

        IsLoading = true;
        
        _eventAggregator.Subscribe<LoadEndEvent>(this);

        SpecflowNavigationCommand = new AsyncCommand(NavigateToSpecflowAsync);
        UnitTestDependencyTypeNavigationCommand = new AsyncCommand(NavigateToUnitTestDependencyTypeAsync);
        UnitTestDependencyManuallyNavigationCommand = new AsyncCommand(NavigateToUnitTestDependencyManuallyAsync);
        ComposerNavigationCommand = new AsyncCommand(NavigateToComposerAsync);
        MapperNavigationCommand = new AsyncCommand(NavigateToMapperAsync);
        BuilderNavigationCommand = new AsyncCommand(NavigateToBuilderAsync);
        AssemblyNavigationCommand = new AsyncCommand(NavigateToAssemblyAsync);
        InvalidateAndReloadCommand = new AsyncCommand(InvalidateAndReloadAsync, CanInvalidateAndReload);
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
    public IAsyncCommand InvalidateAndReloadCommand { get; }
    public ICommand OpenSettingsCommand { get; }

    public ActiveItem ActiveItem
    {
        get => Get<ActiveItem>();
        set => Set(value);
    }
    
    public bool IsLoading
    {
        get => Get<bool>();
        set => Set(value);
    }
    
    public Task HandleAsync(LoadEndEvent e)
    {
        IsLoading = false;
        InvalidateAndReloadCommand.RaiseCanExecuteChanged();
        Log.Debug("Loading Ended {In}", GetType().FullName);
        return Task.CompletedTask;
    }
    
    private async Task InvalidateAndReloadAsync()
    {
        IsLoading = true;
        InvalidateAndReloadCommand.RaiseCanExecuteChanged();
        await _eventAggregator.PublishAsync(new LoadStartEvent()).ConfigureAwait(true);
        var configuration = await _configurationLoader.LoadAsync().ConfigureAwait(true);
        _typeEvaluator.EvaluateTypes(configuration);
        _typeProvider.Initalize(configuration);
        _configurationProvider.SetConfiguration(configuration);
        await _eventAggregator.PublishAsync(new LoadEndEvent()).ConfigureAwait(true);
    }
    
    private bool CanInvalidateAndReload(object? arg)
    {
        return !IsLoading;
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
        var wizardBuilder = _factory.Create<IWizardBuilder>();

        var wizard = wizardBuilder
            //.Then<SetupWizardViewModel>((options, _) => options.AllowNext(_ => true))
            .Then<ConfigurationWizardViewModel>((options, _) => options.AllowBack(_ => true))
            .Build();

        return _settingsWizardRunner.RunAsync(wizard);
    }
}