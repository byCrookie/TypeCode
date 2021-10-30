using System.Threading;
using System.Threading.Tasks;
using Framework.Autofac.Factory;
using Framework.Boot;
using TypeCode.Business.Bootstrapping;
using TypeCode.Business.TypeEvaluation;
using TypeCode.Wpf.Helper.Event;

namespace TypeCode.Wpf
{
    public class Application : IApplication
    {
        private readonly IFactory<MainViewModel> _mainViewModelFactory;
        private readonly ITypeEvaluator _typeEvaluator;
        private readonly ITypeProvider _typeProvider;
        private readonly MainWindow _mainWindow;
        private readonly IEventAggregator _eventAggregator;

        public Application(
            IFactory<MainViewModel> mainViewModelFactory,
            ITypeEvaluator typeEvaluator,
            ITypeProvider typeProvider,
            MainWindow mainWindow,
            IEventAggregator eventAggregator
        )
        {
            _mainViewModelFactory = mainViewModelFactory;
            _typeEvaluator = typeEvaluator;
            _typeProvider = typeProvider;
            _mainWindow = mainWindow;
            _eventAggregator = eventAggregator;
        }

        public Task RunAsync(CancellationToken cancellationToken)
        {
            _mainWindow.DataContext = _mainViewModelFactory.Create();

            Task.Run(LoadAssemblies, cancellationToken);

            _mainWindow.ShowDialog();

            return Task.CompletedTask;
        }

        private Task LoadAssemblies()
        {
            var configuration = _typeEvaluator.EvaluateTypes(AssemblyLoadProvider.GetConfiguration());
            _typeProvider.Initalize(configuration);
            return _eventAggregator.PublishAsync(new AssemblyLoadedEvent());
        }
    }
}