using System.Windows;
using System.Windows.Threading;
using AsyncAwaitBestPractices;
using Framework.Boot;
using Framework.DependencyInjection.Factory;
using Nito.AsyncEx;
using Serilog;
using TypeCode.Business.Configuration;
using TypeCode.Business.TypeEvaluation;
using TypeCode.Wpf.Helper.Event;
using TypeCode.Wpf.Helper.Navigation.Modal.Service;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.Thread;
using TypeCode.Wpf.Main;

namespace TypeCode.Wpf.Application;

public class Application<TContext> : IApplication<TContext> where TContext : BootContext
{
    private readonly IFactory _factory;
    private readonly ITypeProvider _typeProvider;
    private readonly IEventAggregator _eventAggregator;
    private readonly IModalNavigationService _modalNavigationService;
    private readonly IConfigurationProvider _configurationProvider;
    private readonly IMainViewProvider _mainViewProvider;
    private readonly IConfigurationLoader _configurationLoader;

    public Application(
        IFactory factory,
        ITypeProvider typeProvider,
        IEventAggregator eventAggregator,
        IModalNavigationService modalNavigationService,
        IConfigurationProvider configurationProvider,
        IMainViewProvider mainViewProvider,
        IConfigurationLoader configurationLoader
    )
    {
        _factory = factory;
        _typeProvider = typeProvider;
        _eventAggregator = eventAggregator;
        _modalNavigationService = modalNavigationService;
        _configurationProvider = configurationProvider;
        _mainViewProvider = mainViewProvider;
        _configurationLoader = configurationLoader;
    }

    public Task RunAsync(TContext context, CancellationToken cancellationToken)
    {
        var mainWindow = _mainViewProvider.MainWindow();

        System.Windows.Application.Current.DispatcherUnhandledException +=
            (_, args) => HandleDispatcherUnhandledException(args, mainWindow);

        SafeFireAndForgetExtensions.SetDefaultExceptionHandling(e => { HandleException(e, mainWindow); });

        var mainViewModel = _factory.Create<MainViewModel>();
        mainViewModel.OnNavigatedToAsync(new NavigationContext());
        mainWindow.DataContext = mainViewModel;

        LoadAssembliesAsync().SafeFireAndForget();

        mainWindow.ShowDialog();

        return Task.CompletedTask;
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

        MainThread.BackgroundFireAndForget(() =>
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
            await _typeProvider.InitalizeAsync(configuration).ConfigureAwait(false);
            _configurationProvider.Set(configuration);
        }
        finally
        {
            await _eventAggregator.PublishAsync(new LoadEndEvent()).ConfigureAwait(false);
        }
    }
}