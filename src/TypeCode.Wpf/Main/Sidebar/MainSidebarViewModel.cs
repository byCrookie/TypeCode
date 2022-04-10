﻿using System.Windows.Input;
using AsyncAwaitBestPractices.MVVM;
using Framework.DependencyInjection.Factory;
using Serilog;
using TypeCode.Business.Configuration;
using TypeCode.Business.TypeEvaluation;
using TypeCode.Wpf.Application;
using TypeCode.Wpf.Helper.Commands;
using TypeCode.Wpf.Helper.Event;
using TypeCode.Wpf.Helper.Navigation.Contract;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.Navigation.Wizard.Complex;
using TypeCode.Wpf.Helper.ViewModel;
using TypeCode.Wpf.Pages.Assemblies;
using TypeCode.Wpf.Pages.Builder;
using TypeCode.Wpf.Pages.Common.Configuration;
using TypeCode.Wpf.Pages.Composer;
using TypeCode.Wpf.Pages.Home;
using TypeCode.Wpf.Pages.Mapper;
using TypeCode.Wpf.Pages.Specflow;
using TypeCode.Wpf.Pages.UnitTestDependencyManually;
using TypeCode.Wpf.Pages.UnitTestDependencyType;

namespace TypeCode.Wpf.Main.Sidebar;

public class MainSidebarViewModel : Reactive, IAsyncNavigatedTo, IAsyncEventHandler<LoadEndEvent>
{
    private readonly INavigationService _navigationService;
    private readonly IFactory _factory;
    private readonly IWizardRunner _settingsWizardRunner;
    private readonly IEventAggregator _eventAggregator;
    private readonly IConfigurationProvider _configurationProvider;
    private readonly IConfigurationLoader _configurationLoader;
    private readonly ITypeProvider _typeProvider;

    public MainSidebarViewModel(
        INavigationService navigationService,
        IFactory factory,
        IWizardRunner settingsWizardRunner,
        IEventAggregator eventAggregator,
        IConfigurationProvider configurationProvider,
        IConfigurationLoader configurationLoader,
        ITypeProvider typeProvider
    )
    {
        _navigationService = navigationService;
        _factory = factory;
        _settingsWizardRunner = settingsWizardRunner;
        _eventAggregator = eventAggregator;
        _configurationProvider = configurationProvider;
        _configurationLoader = configurationLoader;
        _typeProvider = typeProvider;

        IsLoading = true;

        _eventAggregator.Subscribe<LoadEndEvent>(this);

        HomeNavigationCommand = new AsyncRelayCommand(NavigateToHomeAsync);
        SpecflowNavigationCommand = new AsyncRelayCommand(NavigateToSpecflowAsync);
        UnitTestDependencyTypeNavigationCommand = new AsyncRelayCommand(NavigateToUnitTestDependencyTypeAsync);
        UnitTestDependencyManuallyNavigationCommand = new AsyncRelayCommand(NavigateToUnitTestDependencyManuallyAsync);
        ComposerNavigationCommand = new AsyncRelayCommand(NavigateToComposerAsync);
        MapperNavigationCommand = new AsyncRelayCommand(NavigateToMapperAsync);
        BuilderNavigationCommand = new AsyncRelayCommand(NavigateToBuilderAsync);
        AssemblyNavigationCommand = new AsyncRelayCommand(NavigateToAssemblyAsync);
        InvalidateAndReloadCommand = new AsyncRelayCommand(InvalidateAndReloadAsync, CanInvalidateAndReload);
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
    
    public Task OnNavigatedToAsync(NavigationContext context)
    {
        return NavigateToHomeAsync();
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
        await Task.Run(async () =>
        {
            var configuration = await _configurationLoader.LoadAsync().ConfigureAwait(false);
            await _typeProvider.InitalizeAsync(configuration).ConfigureAwait(false);
            _configurationProvider.Set(configuration);
            await _eventAggregator.PublishAsync(new LoadEndEvent()).ConfigureAwait(false);
        }).ConfigureAwait(false);
    }

    private bool CanInvalidateAndReload(object? arg)
    {
        return !IsLoading;
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

    private Task OpenSettingsAsync()
    {
        var wizardBuilder = _factory.Create<IWizardBuilder>();

        var wizard = wizardBuilder
            .Then<SetupWizardViewModel>((options, _) => options.AllowNext(_ => true))
            .Build();

        return _settingsWizardRunner.RunAsync(wizard);
    }
}