using AsyncAwaitBestPractices.MVVM;
using DependencyInjection.Factory;
using Serilog;
using TypeCode.Business.Configuration;
using TypeCode.Business.TypeEvaluation;
using TypeCode.Wpf.Application;
using TypeCode.Wpf.Components.InfoLink;
using TypeCode.Wpf.Helper.Commands;
using TypeCode.Wpf.Helper.Event;
using TypeCode.Wpf.Helper.Navigation.Contract;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.ViewModel;
using TypeCode.Wpf.Main.Content;
using TypeCode.Wpf.Main.Sidebar;

namespace TypeCode.Wpf.Main;

public class MainViewModel :
    Reactive,
    IAsyncNavigatedTo,
    IAsyncEventHandler<VersionLoadedEvent>,
    IAsyncEventHandler<LoadEndEvent>
{
    private readonly IEventAggregator _eventAggregator;
    private readonly IConfigurationProvider _configurationProvider;
    private readonly IConfigurationLoader _configurationLoader;
    private readonly ITypeProvider _typeProvider;
    private readonly IInfoLinkViewModelFactory _infoLinkViewModelFactory;
    private readonly NavigationContext _navigationContext;
    private bool _isLoading;

    public MainViewModel(
        IFactory factory,
        IEventAggregator eventAggregator,
        IConfigurationProvider configurationProvider,
        IConfigurationLoader configurationLoader,
        ITypeProvider typeProvider,
        IInfoLinkViewModelFactory infoLinkViewModelFactory)
    {
        _eventAggregator = eventAggregator;
        _configurationProvider = configurationProvider;
        _configurationLoader = configurationLoader;
        _typeProvider = typeProvider;
        _infoLinkViewModelFactory = infoLinkViewModelFactory;

        _isLoading = true;

        eventAggregator.Subscribe<LoadEndEvent>(this);
        eventAggregator.Subscribe<VersionLoadedEvent>(this);

        Title = "TypeCode";

        MainContentViewModel = factory.Create<MainContentViewModel>();
        MainSidebarViewModel = factory.Create<MainSidebarViewModel>();

        _navigationContext = new NavigationContext();

        InvalidateAndReloadCommand = new AsyncRelayCommand(InvalidateAndReloadAsync, CanInvalidateAndReload);
    }

    public Task OnNavigatedToAsync(NavigationContext context)
    {
        InfoLink = _infoLinkViewModelFactory
            .Create(new InfoLinkViewModelParameter("https://github.com/byCrookie/TypeCode/wiki"));

        if (MainSidebarViewModel is IAsyncNavigatedTo navSideBar)
        {
            navSideBar.OnNavigatedToAsync(_navigationContext);
        }

        return Task.CompletedTask;
    }

    public IAsyncCommand InvalidateAndReloadCommand { get; }

    public MainContentViewModel MainContentViewModel { get; }
    public MainSidebarViewModel MainSidebarViewModel { get; }

    public string? Title
    {
        get => Get<string?>();
        set => Set(value);
    }

    public InfoLinkViewModel? InfoLink
    {
        get => Get<InfoLinkViewModel?>();
        set => Set(value);
    }

    public Task HandleAsync(VersionLoadedEvent e)
    {
        Title = $"TypeCode {e.CurrentVersion}";
        return Task.CompletedTask;
    }

    public Task HandleAsync(LoadEndEvent e)
    {
        _isLoading = false;
        InvalidateAndReloadCommand.RaiseCanExecuteChanged();
        Log.Debug("Loading Ended {In}", GetType().FullName);
        return Task.CompletedTask;
    }

    private async Task InvalidateAndReloadAsync()
    {
        _isLoading = true;
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
        return !_isLoading;
    }
}