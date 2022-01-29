using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using Framework.DependencyInjection.Factory;
using Framework.Jab.Boot;
using Nito.AsyncEx;
using TypeCode.Business.Bootstrapping;
using TypeCode.Business.TypeEvaluation;
using TypeCode.Wpf.Application.Boot.SetupWpfApplication;
using TypeCode.Wpf.Helper.Event;
using TypeCode.Wpf.Helper.Navigation.Modal.Service;
using TypeCode.Wpf.Main;

namespace TypeCode.Wpf.Application;

public class Application : IApplication
{
    private readonly IFactory _factory;
    private readonly ITypeEvaluator _typeEvaluator;
    private readonly ITypeProvider _typeProvider;
    private readonly IEventAggregator _eventAggregator;
    private readonly IModalNavigationService _modalNavigationService;
    private readonly IWpfWindowProvider _windowProvider;

    public Application(
        IFactory factory,
        ITypeEvaluator typeEvaluator,
        ITypeProvider typeProvider,
        IEventAggregator eventAggregator,
        IModalNavigationService modalNavigationService,
        IWpfWindowProvider windowProvider
    )
    {
        _factory = factory;
        _typeEvaluator = typeEvaluator;
        _typeProvider = typeProvider;
        _eventAggregator = eventAggregator;
        _modalNavigationService = modalNavigationService;
        _windowProvider = windowProvider;
    }

    public Task RunAsync(CancellationToken cancellationToken)
    {
        System.Windows.Application.Current.DispatcherUnhandledException += HandleDispatcherUnhandledException;

        var mainWindow = _windowProvider.Get().MainWindow;
        var mainViewModel = _factory.Create<MainViewModel>();

        mainWindow.DataContext = mainViewModel;

        Task.Run(LoadAssembliesAsync, cancellationToken);

        mainWindow.ShowDialog();

        return Task.CompletedTask;
    }

    private void HandleDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        e.Handled = true;
        AsyncContext.Run(() => _modalNavigationService.OpenModalAsync(new ModalParameter
        {
            Title = "ERROR", 
            Text = $"{e.Exception.Message}" +
                   $"{Environment.NewLine}" +
                   $"{e.Exception.InnerException?.Message}" +
                   $"{Environment.NewLine}" +
                   $"{e.Exception.StackTrace}"
        }));
    }

    private Task LoadAssembliesAsync()
    {
        var configuration = _typeEvaluator.EvaluateTypes(AssemblyLoadProvider.GetConfiguration());
        _typeProvider.Initalize(configuration);
        return _eventAggregator.PublishAsync(new AssemblyLoadedEvent());
    }
}