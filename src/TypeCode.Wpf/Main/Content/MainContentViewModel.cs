using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Windows.Threading;
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
using TypeCode.Wpf.Helper.Thread;
using TypeCode.Wpf.Helper.ViewModels;
using System.Diagnostics;
using TypeCode.Wpf.Helper.Navigation.Modal.Service;

namespace TypeCode.Wpf.Main.Content;

public sealed partial class MainContentViewModel :
    ViewModelBase,
    IAsyncEventHandler<LoadStartEvent>,
    IAsyncEventHandler<LoadEndEvent>,
    IAsyncEventHandler<BannerOpenEvent>,
    IAsyncNavigatedTo,
    IAsyncNavigatedFrom
{
    private readonly IEventAggregator _eventAggregator;
    private readonly IVersionEvaluator _versionEvaluator;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IModalNavigationService _modalNavigationService;

    private Guid? _currentBanner;
    private VersionResult? _version;

    public MainContentViewModel(
        IEventAggregator eventAggregator,
        IVersionEvaluator versionEvaluator,
        IDateTimeProvider dateTimeProvider,
        IModalNavigationService modalNavigationService)
    {
        _eventAggregator = eventAggregator;
        _versionEvaluator = versionEvaluator;
        _dateTimeProvider = dateTimeProvider;
        _modalNavigationService = modalNavigationService;
    }

    public Task OnNavigatedToAsync(NavigationContext context)
    {
        IsLoading = true;
        LoadingStarted = _dateTimeProvider.Now();

        _eventAggregator.Subscribe<BannerOpenEvent>(this);
        _eventAggregator.Subscribe<LoadStartEvent>(this);
        _eventAggregator.Subscribe<LoadEndEvent>(this);

        CheckVersionAsync().SafeFireAndForget();

        return Task.CompletedTask;
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

    [RelayCommand]
    private Task UpdateAsync()
    {
        if (_version is null)
        {
            throw new Exception("There is no new version available.");
        }

        return _modalNavigationService.OpenModalAsync(new ModalParameter
        {
            Title = "WARNING - Update (Installer)",
            Text = "The installer uses the location of the current version of TypeCode to install the update." +
                   " You can change the installation path (destination folder) in the advanced menu of the install wizard.",
            OnOkAsync = async () =>
            {
                IsBannerVisible = false;
                
                var name = $"TypeCode.Wpf.Setup_{_version?.NewVersion}";

                var url = $"https://github.com/byCrookie/TypeCode/releases/download/{_version?.NewVersion}/{name}.msi";
                var executingLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? throw new Exception();

                var msi = Path.Combine(executingLocation, $"{name}.msi");

                if (File.Exists(msi))
                {
                    File.Delete(msi);
                }

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("User-Agent", "byCrookie");

                    var uri = new Uri(url);

                    var response = await client.GetAsync(uri).ConfigureAwait(true);

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception("Error retrieving update package");
                    }

                    var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(true);

                    await using (var fileStream = new FileStream(msi, FileMode.Create))
                    {
                        await stream.CopyToAsync(fileStream).ConfigureAwait(true);
                    }
                }

                var process = new Process();
                process.StartInfo.FileName = "msiexec";
                process.StartInfo.Arguments = $" /i {msi} APPLICATIONFOLDER={executingLocation}";
                process.StartInfo.Verb = "runas";
                process.Start();
                Environment.Exit(0);
            }
        });
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

    public Task HandleAsync(LoadStartEvent e, CancellationToken? ct = null)
    {
        IsLoading = true;
        LoadingStarted = _dateTimeProvider.Now();
        return Task.CompletedTask;
    }

    public Task HandleAsync(LoadEndEvent e, CancellationToken? ct = null)
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

    public Task HandleAsync(BannerOpenEvent e, CancellationToken? ct = null)
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

            MainThread.BackgroundFireAndForgetAsync(() => _version = version, DispatcherPriority.Normal);
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