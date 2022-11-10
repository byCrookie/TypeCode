using System.Windows;
using System.Windows.Threading;
using AsyncAwaitBestPractices;
using DependencyInjection.Factory;
using Framework.Boot;
using Nito.AsyncEx;
using Serilog;
using TypeCode.Business.Configuration;
using TypeCode.Business.TypeEvaluation;
using TypeCode.Wpf.Helper.Event;
using TypeCode.Wpf.Helper.Navigation.Modal.Service;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.Thread;
using TypeCode.Wpf.Helper.ViewModels;
using TypeCode.Wpf.Main;

namespace TypeCode.Wpf.Application;

public sealed class Application<TContext> : IApplication<TContext> where TContext : BootContext
{
    private readonly IFactory<MainViewModel> _mainViewModelFactory;
    private readonly IEventAggregator _eventAggregator;
    private readonly IModalNavigationService _modalNavigationService;
    private readonly IConfigurationProvider _configurationProvider;
    private readonly IMainViewProvider _mainViewProvider;
    private readonly IConfigurationLoader _configurationLoader;
    private readonly ILazyTypeProviderFactory _lazyTypeProviderFactory;

    public Application(
        IFactory<MainViewModel> mainViewModelFactory,
        IEventAggregator eventAggregator,
        IModalNavigationService modalNavigationService,
        IConfigurationProvider configurationProvider,
        IMainViewProvider mainViewProvider,
        IConfigurationLoader configurationLoader,
        ILazyTypeProviderFactory lazyTypeProviderFactory
    )
    {
        _mainViewModelFactory = mainViewModelFactory;
        _eventAggregator = eventAggregator;
        _modalNavigationService = modalNavigationService;
        _configurationProvider = configurationProvider;
        _mainViewProvider = mainViewProvider;
        _configurationLoader = configurationLoader;
        _lazyTypeProviderFactory = lazyTypeProviderFactory;
    }

    public async Task RunAsync(TContext context, CancellationToken ct)
    {
        var mainWindow = _mainViewProvider.MainWindow();

        System.Windows.Application.Current.DispatcherUnhandledException +=
            (_, args) => HandleDispatcherUnhandledException(args, mainWindow);

        SafeFireAndForgetExtensions.SetDefaultExceptionHandling(e => { HandleException(e, mainWindow); });

        var mainViewModel = _mainViewModelFactory.Create();
        mainWindow.DataContext = mainViewModel;
        await NavigationCaller.CallNavigateToAsync(mainViewModel, new NavigationContext()).ConfigureAwait(true);

        LoadAssembliesAsync().SafeFireAndForget();

        mainWindow.ShowDialog();
    }

    private static void CloseOverlays(MainWindow mainWindow)
    {
        mainWindow.MainContent.Opacity = 1;
        mainWindow.MainContent.IsEnabled = true;
        mainWindow.WizardOverlay.Visibility = Visibility.Collapsed;
    }

    private void HandleDispatcherUnhandledException(DispatcherUnhandledExceptionEventArgs e, MainWindow mainWindow)
    {
        e.Handled = true;
        HandleException(e.Exception, mainWindow);
    }

    private void HandleException(Exception exception, MainWindow mainWindow)
    {
        Log.Error(exception, "{0}", exception.Message);

        MainThread.BackgroundFireAndForgetSync(() =>
        {
            CloseOverlays(mainWindow);

            AsyncContext.Run(() => _modalNavigationService.OpenModalAsync(new ModalParameter
            {
                Title = "ERROR",
                Text = $"{exception.Message}" +
                       $"{Environment.NewLine}" +
                       $"{exception.InnerException?.Message}" +
                       $"{Environment.NewLine}" +
                       $"{exception.StackTrace}"
            }));
        }, DispatcherPriority.Send);
    }

    private async Task LoadAssembliesAsync()
    {
        try
        {
            var configuration = await _configurationLoader.LoadAsync().ConfigureAwait(false);
            _lazyTypeProviderFactory.InitializeByConfiguration(configuration);
            await _lazyTypeProviderFactory.ValueAsync().ConfigureAwait(false);
            _configurationProvider.Set(configuration);
        }
        finally
        {
            await _eventAggregator.PublishAsync(new LoadEndEvent()).ConfigureAwait(false);
        }
    }
}