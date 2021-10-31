﻿using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using Framework.Autofac.Factory;
using Framework.Boot;
using Nito.AsyncEx;
using TypeCode.Business.Bootstrapping;
using TypeCode.Business.TypeEvaluation;
using TypeCode.Wpf.Helper.Event;
using TypeCode.Wpf.Helper.Modal;
using TypeCode.Wpf.Helper.Navigation;

namespace TypeCode.Wpf
{
    public class Application : IApplication
    {
        private readonly IFactory<MainViewModel> _mainViewModelFactory;
        private readonly ITypeEvaluator _typeEvaluator;
        private readonly ITypeProvider _typeProvider;
        private readonly MainWindow _mainWindow;
        private readonly IEventAggregator _eventAggregator;
        private readonly INavigationService _navigationService;

        public Application(
            IFactory<MainViewModel> mainViewModelFactory,
            ITypeEvaluator typeEvaluator,
            ITypeProvider typeProvider,
            MainWindow mainWindow,
            IEventAggregator eventAggregator,
            INavigationService navigationService
        )
        {
            _mainViewModelFactory = mainViewModelFactory;
            _typeEvaluator = typeEvaluator;
            _typeProvider = typeProvider;
            _mainWindow = mainWindow;
            _eventAggregator = eventAggregator;
            _navigationService = navigationService;
        }

        public Task RunAsync(CancellationToken cancellationToken)
        {
            System.Windows.Application.Current.DispatcherUnhandledException += HandleDispatcherUnhandledException;

            _mainWindow.DataContext = _mainViewModelFactory.Create();

            Task.Run(LoadAssemblies, cancellationToken);

            _mainWindow.ShowDialog();

            return Task.CompletedTask;
        }

        private void HandleDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            AsyncContext.Run(() => _navigationService.OpenModal(new ModalParameter
            {
                Title = "ERROR", 
                Text = e.Exception.Message
            }));
        }

        private Task LoadAssemblies()
        {
            var configuration = _typeEvaluator.EvaluateTypes(AssemblyLoadProvider.GetConfiguration());
            _typeProvider.Initalize(configuration);
            return _eventAggregator.PublishAsync(new AssemblyLoadedEvent());
        }
    }
}