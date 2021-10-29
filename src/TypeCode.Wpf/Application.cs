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

        public Application(
            IFactory<MainViewModel> mainViewModelFactory,
            ITypeEvaluator typeEvaluator,
            ITypeProvider typeProvider
        )
        {
            _mainViewModelFactory = mainViewModelFactory;
            _typeEvaluator = typeEvaluator;
            _typeProvider = typeProvider;
        }

        public Task RunAsync(CancellationToken cancellationToken)
        {
            var mainView = new MainWindow
            {
                DataContext = _mainViewModelFactory.Create()
            };
            
            Task.Run(() =>
            {
                var configuration = _typeEvaluator.EvaluateTypes(AssemblyLoadProvider.GetConfiguration());
                _typeProvider.Initalize(configuration);
            }, cancellationToken);
            
            mainView.ShowDialog();

            // Task.Delay(Timeout.Infinite, cancellationToken)
            return Task.CompletedTask;
        }
    }
}