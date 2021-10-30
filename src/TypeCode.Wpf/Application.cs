using System.Threading;
using System.Threading.Tasks;
using Framework.Autofac.Factory;
using Framework.Boot;
using TypeCode.Business.Bootstrapping;
using TypeCode.Business.TypeEvaluation;

namespace TypeCode.Wpf
{
    public class Application : IApplication
    {
        private readonly IFactory<MainViewModel> _mainViewModelFactory;
        private readonly ITypeEvaluator _typeEvaluator;
        private readonly ITypeProvider _typeProvider;
        private readonly MainWindow _mainWindow;

        public Application(
            IFactory<MainViewModel> mainViewModelFactory,
            ITypeEvaluator typeEvaluator,
            ITypeProvider typeProvider,
            MainWindow mainWindow
        )
        {
            _mainViewModelFactory = mainViewModelFactory;
            _typeEvaluator = typeEvaluator;
            _typeProvider = typeProvider;
            _mainWindow = mainWindow;
        }

        public Task RunAsync(CancellationToken cancellationToken)
        {
            _mainWindow.DataContext = _mainViewModelFactory.Create();

            Task.Run(() =>
            {
                var configuration = _typeEvaluator.EvaluateTypes(AssemblyLoadProvider.GetConfiguration());
                _typeProvider.Initalize(configuration);
            }, cancellationToken);

            _mainWindow.ShowDialog();

            return Task.CompletedTask;
        }
    }
}