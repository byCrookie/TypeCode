using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DependencyInjection.Factory;
using Serilog;
using TypeCode.Business.Configuration;
using TypeCode.Business.TypeEvaluation;
using TypeCode.Wpf.Application;
using TypeCode.Wpf.Components.InfoLink;
using TypeCode.Wpf.Helper.Event;
using TypeCode.Wpf.Helper.Navigation.Contract;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Main.Content;
using TypeCode.Wpf.Main.Sidebar;

namespace TypeCode.Wpf.Main;

public partial class MainViewModel :
    ObservableObject,
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

    [ObservableProperty]
    private MainContentViewModel? _mainContentViewModel;

    [ObservableProperty]
    private MainSidebarViewModel? _mainSidebarViewModel;

    [ObservableProperty]
    private string? _title;

    [ObservableProperty]
    private InfoLinkViewModel? _infoLink;

    public Task HandleAsync(VersionLoadedEvent e)
    {
        Title = $"TypeCode {e.CurrentVersion}";
        return Task.CompletedTask;
    }

    public Task HandleAsync(LoadEndEvent e)
    {
        _isLoading = false;
        InvalidateAndReloadCommand.NotifyCanExecuteChanged();
        Log.Debug("Loading Ended {In}", GetType().FullName);
        return Task.CompletedTask;
    }

    [RelayCommand(CanExecute = nameof(CanInvalidateAndReload))]
    private async Task InvalidateAndReloadAsync()
    {
        _isLoading = true;
        InvalidateAndReloadCommand.NotifyCanExecuteChanged();
        await _eventAggregator.PublishAsync(new LoadStartEvent()).ConfigureAwait(true);
        await Task.Run(async () =>
        {
            var configuration = await _configurationLoader.LoadAsync().ConfigureAwait(false);
            await _typeProvider.InitalizeAsync(configuration).ConfigureAwait(false);
            _configurationProvider.Set(configuration);
            await _eventAggregator.PublishAsync(new LoadEndEvent()).ConfigureAwait(false);
        }).ConfigureAwait(false);
    }

    private bool CanInvalidateAndReload()
    {
        return !_isLoading;
    }
}