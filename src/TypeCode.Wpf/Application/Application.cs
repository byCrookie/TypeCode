using System.Windows;
using System.Windows.Threading;
using AsyncAwaitBestPractices;
using Framework.Autofac.Boot;
using Framework.DependencyInjection.Factory;
using Nito.AsyncEx;
using TypeCode.Business.Bootstrapping;
using TypeCode.Business.TypeEvaluation;
using TypeCode.Wpf.Helper.Event;
using TypeCode.Wpf.Helper.Navigation.Modal.Service;
using TypeCode.Wpf.Main;

namespace TypeCode.Wpf.Application;

public class Application<TContext> : IApplication<TContext> where TContext : BootContext
{
    private readonly IFactory _factory;
    private readonly ITypeEvaluator _typeEvaluator;
    private readonly ITypeProvider _typeProvider;
    private readonly IEventAggregator _eventAggregator;
    private readonly IModalNavigationService _modalNavigationService;

    public Application(
        IFactory factory,
        ITypeEvaluator typeEvaluator,
        ITypeProvider typeProvider,
        IEventAggregator eventAggregator,
        IModalNavigationService modalNavigationService
    )
    {
        _factory = factory;
        _typeEvaluator = typeEvaluator;
        _typeProvider = typeProvider;
        _eventAggregator = eventAggregator;
        _modalNavigationService = modalNavigationService;
    }

    public Task RunAsync(TContext context, CancellationToken cancellationToken)
    {
        var mainWindow = _factory.Create<MainWindow>();
            
        System.Windows.Application.Current.DispatcherUnhandledException +=
            (_, args) => HandleDispatcherUnhandledException(args, mainWindow);
        
        SafeFireAndForgetExtensions.SetDefaultExceptionHandling(e =>
        {
            OpenExceptionDialog(e, mainWindow);
        });
        
        var mainViewModel = _factory.Create<MainViewModel>();

        mainWindow.DataContext = mainViewModel;

        Task.Run(LoadAssembliesAsync, cancellationToken);

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
        OpenExceptionDialog(e.Exception, mainWindow);
    }

    private void OpenExceptionDialog(Exception exception, MainWindow mainWindow)
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
    }

    private Task LoadAssembliesAsync()
    {
        var configuration = _typeEvaluator.EvaluateTypes(AssemblyLoadProvider.GetConfiguration());
        _typeProvider.Initalize(configuration);
        return _eventAggregator.PublishAsync(new AssemblyLoadedEvent());
    }
}