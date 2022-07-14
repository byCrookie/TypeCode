using System.Windows.Input;
using AsyncAwaitBestPractices;
using Framework.Time;
using Serilog;
using TypeCode.Business.Version;
using TypeCode.Wpf.Application;
using TypeCode.Wpf.Helper.Commands;
using TypeCode.Wpf.Helper.Event;
using TypeCode.Wpf.Helper.ViewModel;

namespace TypeCode.Wpf.Main.Content;

public class MainContentViewModel :
    Reactive,
    IAsyncEventHandler<LoadStartEvent>,
    IAsyncEventHandler<LoadEndEvent>,
    IAsyncEventHandler<BannerOpenEvent>
{
    private readonly IEventAggregator _eventAggregator;
    private readonly IVersionEvaluator _versionEvaluator;
    private readonly IDateTimeProvider _dateTimeProvider;

    public MainContentViewModel(IEventAggregator eventAggregator, IVersionEvaluator versionEvaluator, IDateTimeProvider dateTimeProvider)
    {
        _eventAggregator = eventAggregator;
        _versionEvaluator = versionEvaluator;
        _dateTimeProvider = dateTimeProvider;

        IsLoading = true;
        LoadingStarted = _dateTimeProvider.Now();

        eventAggregator.Subscribe<BannerOpenEvent>(this);
        eventAggregator.Subscribe<LoadStartEvent>(this);
        eventAggregator.Subscribe<LoadEndEvent>(this);

        CloseBannerCommand = new AsyncRelayCommand(() =>
        {
            IsBannerVisible = false;
            return Task.CompletedTask;
        });

        CheckVersionAsync().SafeFireAndForget();
    }

    public bool IsLoading
    {
        get => Get<bool>();
        set => Set(value);
    }

    public DateTime LoadingStarted
    {
        get => Get<DateTime>();
        set => Set(value);
    }

    public bool IsBannerVisible
    {
        get => Get<bool>();
        set => Set(value);
    }

    public bool IsBannerLink
    {
        get => Get<bool>();
        set => Set(value);
    }

    public string? BannerLink
    {
        get => Get<string?>();
        set => Set(value);
    }

    public string? BannerMessage
    {
        get => Get<string?>();
        set => Set(value);
    }

    public ICommand CloseBannerCommand { get; }
    private Guid? CurrentBanner { get; set; }

    public Task HandleAsync(LoadStartEvent e)
    {
        IsLoading = true;
        LoadingStarted = _dateTimeProvider.Now();
        return Task.CompletedTask;
    }

    public Task HandleAsync(LoadEndEvent e)
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

    public Task HandleAsync(BannerOpenEvent e)
    {
        IsBannerLink = e.IsLink;
        BannerLink = e.Link;
        BannerMessage = e.Message;

        IsBannerVisible = true;
        CurrentBanner = Guid.NewGuid();

        if (e.VisibleTime is not null)
        {
            HideBannerAsync(CurrentBanner, e.VisibleTime.Value).SafeFireAndForget();
        }

        return Task.CompletedTask;
    }

    private async Task HideBannerAsync(Guid? currentBanner, TimeSpan timeSpan)
    {
        await Task.Delay(timeSpan).ConfigureAwait(false);
        if (CurrentBanner == currentBanner)
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