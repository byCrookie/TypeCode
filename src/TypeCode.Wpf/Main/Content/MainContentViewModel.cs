using AsyncAwaitBestPractices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Framework.Time;
using Serilog;
using TypeCode.Business.Version;
using TypeCode.Wpf.Application;
using TypeCode.Wpf.Helper.Event;
using TypeCode.Wpf.Helper.Navigation.Contract;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.ViewModels;

namespace TypeCode.Wpf.Main.Content;

public partial class MainContentViewModel :
    ViewModelBase,
    IAsyncEventHandler<LoadStartEvent>,
    IAsyncEventHandler<LoadEndEvent>,
    IAsyncEventHandler<BannerOpenEvent>,
    IAsyncNavigatedFrom
{
    private readonly IEventAggregator _eventAggregator;
    private readonly IVersionEvaluator _versionEvaluator;
    private readonly IDateTimeProvider _dateTimeProvider;

    private Guid? _currentBanner;

    public MainContentViewModel(
        IEventAggregator eventAggregator,
        IVersionEvaluator versionEvaluator,
        IDateTimeProvider dateTimeProvider)
    {
        _eventAggregator = eventAggregator;
        _versionEvaluator = versionEvaluator;
        _dateTimeProvider = dateTimeProvider;

        IsLoading = true;
        LoadingStarted = _dateTimeProvider.Now();

        eventAggregator.Subscribe<BannerOpenEvent>(this);
        eventAggregator.Subscribe<LoadStartEvent>(this);
        eventAggregator.Subscribe<LoadEndEvent>(this);

        CheckVersionAsync().SafeFireAndForget();
    }
    
    public Task OnNavigatedFromAsync(NavigationContext context)
    {
        _eventAggregator.Unsubscribe(this);
        return Task.CompletedTask;
    }

    [RelayCommand]
    private Task CloseBannerAsync()
    {
        IsBannerVisible = false;
        return Task.CompletedTask;
    }

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private DateTime _loadingStarted;

    [ObservableProperty]
    private bool _isBannerVisible;

    [ObservableProperty]
    private bool _isBannerLink;

    [ObservableProperty]
    private string? _bannerLink;

    [ObservableProperty]
    private string? _bannerMessage;

    public Task HandleAsync(LoadStartEvent e, CancellationToken? cancellationToken = null)
    {
        IsLoading = true;
        LoadingStarted = _dateTimeProvider.Now();
        return Task.CompletedTask;
    }

    public Task HandleAsync(LoadEndEvent e, CancellationToken? cancellationToken = null)
    {
        var diff = _dateTimeProvider.Now() - LoadingStarted;

        if (diff >= TimeSpan.FromSeconds(1))
        {
            IsLoading = false;
        }
        else
        {
            Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(1) - diff).ConfigureAwait(true);
                IsLoading = false;
            });
        }

        Log.Debug("Loading Ended {In}", GetType().FullName);
        return Task.CompletedTask;
    }

    public Task HandleAsync(BannerOpenEvent e, CancellationToken? cancellationToken = null)
    {
        IsBannerLink = e.IsLink;
        BannerLink = e.Link;
        BannerMessage = e.Message;

        IsBannerVisible = true;
        _currentBanner = Guid.NewGuid();

        if (e.VisibleTime is not null)
        {
            HideBannerAsync(_currentBanner, e.VisibleTime.Value).SafeFireAndForget();
        }

        return Task.CompletedTask;
    }

    private async Task HideBannerAsync(Guid? currentBanner, TimeSpan timeSpan)
    {
        await Task.Delay(timeSpan).ConfigureAwait(false);
        if (_currentBanner == currentBanner)
        {
            IsBannerVisible = false;
        }
    }

    private async Task CheckVersionAsync()
    {
        try
        {
            var version = await _versionEvaluator.EvaluateAsync().ConfigureAwait(false);

            if (version.CurrentVersion is not null)
            {
                await _eventAggregator.PublishAsync(new VersionLoadedEvent(version.CurrentVersion)).ConfigureAwait(false);
            }

            if (version.NewVersion is null)
            {
                return;
            }

            var versionMessage = $"New version {version.NewVersion} available at https://github.com/byCrookie/TypeCode/releases/tag/{version.NewVersion}";
            var versionLink = $"https://github.com/byCrookie/TypeCode/releases/tag/{version.NewVersion}";

            await _eventAggregator.PublishAsync(new BannerOpenEvent
            {
                Link = versionLink,
                Message = versionMessage,
                IsLink = true
            }).ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            Log.Warning("{Message} {InnerMessage} {Stacktrace}", exception.Message, exception.InnerException?.Message, exception.StackTrace);
            await _eventAggregator.PublishAsync(new BannerOpenEvent
            {
                Link = "https://github.com/byCrookie/TypeCode/releases",
                Message = "Evaluating latest version failed. Manually check https://github.com/byCrookie/TypeCode/releases.",
                IsLink = true,
                VisibleTime = TimeSpan.FromSeconds(15)
            }).ConfigureAwait(false);
        }
    }
}