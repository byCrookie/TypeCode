using System.Windows.Input;
using AsyncAwaitBestPractices;
using AsyncAwaitBestPractices.MVVM;
using Serilog;
using TypeCode.Business.Version;
using TypeCode.Wpf.Application;
using TypeCode.Wpf.Helper.Event;
using TypeCode.Wpf.Helper.ViewModel;

namespace TypeCode.Wpf.Main.Content;

public class MainContentViewModel :
    Reactive,
    IAsyncEventHandler<AssemblyLoadedEvent>,
    IAsyncEventHandler<BannerOpenEvent>
{
    private readonly IEventAggregator _eventAggregator;
    private readonly IVersionEvaluator _versionEvaluator;

    public MainContentViewModel(IEventAggregator eventAggregator, IVersionEvaluator versionEvaluator)
    {
        _eventAggregator = eventAggregator;
        _versionEvaluator = versionEvaluator;

        AreAssembliesLoading = true;

        eventAggregator.Subscribe<BannerOpenEvent>(this);
        eventAggregator.Subscribe<AssemblyLoadedEvent>(this);

        CloseBannerCommand = new AsyncCommand(() =>
        {
            IsBannerVisible = false;
            return Task.CompletedTask;
        });

        CheckVersionAsync().SafeFireAndForget();
    }

    public bool AreAssembliesLoading
    {
        get => Get<bool>();
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

    public ICommand CloseBannerCommand { get; set; }
    private Guid? CurrentBanner { get; set; }

    public Task HandleAsync(AssemblyLoadedEvent e)
    {
        AreAssembliesLoading = false;
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
        await Task.Delay(timeSpan).ConfigureAwait(true);
        if (CurrentBanner == currentBanner)
        {
            IsBannerVisible = false;
        }
    }

    private async Task CheckVersionAsync()
    {
        try
        {
            var version = await _versionEvaluator.EvaluateAsync().ConfigureAwait(true);

            if (version is null)
            {
                return;
            }

            var versionMessage = $"New version {version} available at https://github.com/byCrookie/TypeCode/releases/tag/{version}";
            var versionLink = $"https://github.com/byCrookie/TypeCode/releases/tag/{version}";

            await _eventAggregator.PublishAsync(new BannerOpenEvent
            {
                Link = versionLink,
                Message = versionMessage,
                IsLink = true
            }).ConfigureAwait(true);
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
            }).ConfigureAwait(true);
        }
    }
}