using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TypeCode.Business.Mode;
using TypeCode.Business.Mode.Specflow;
using TypeCode.Business.Mode.UnitTestDependency.Type;
using TypeCode.Business.TypeEvaluation;
using TypeCode.Wpf.Helper.Commands;
using TypeCode.Wpf.Helper.ViewModel;

namespace TypeCode.Wpf.UnitTestDependencyType
{
    public class UnitTestDependencyTypeViewModel : Reactive
    {
        private readonly ITypeCodeGenerator<UnitTestDependencyTypeGeneratorParameter> _unitTestDependencyTypeGenerator;
        private readonly ITypeProvider _typeProvider;

        public UnitTestDependencyTypeViewModel(
            ITypeCodeGenerator<UnitTestDependencyTypeGeneratorParameter> unitTestDependencyTypeGenerator,
            ITypeProvider typeProvider
        )
        {
            _unitTestDependencyTypeGenerator = unitTestDependencyTypeGenerator;
            _typeProvider = typeProvider;

            GenerateCommand = new AsyncRelayCommand(GenerateAsync);
        }

        private async Task GenerateAsync(object arg)
        {
            var inputNames = Input.Split(',').Select(name => name.Trim()).ToList();
            
            var parameter = new UnitTestDependencyTypeGeneratorParameter
            {
                Types = _typeProvider.TryGetByNames(inputNames).ToList()
            };
            
            var result = await _unitTestDependencyTypeGenerator.GenerateAsync(parameter);
            Output = result;
        }
        
        public ICommand GenerateCommand { get; }
        
        public string Input {
            get => Get<string>();
            set => Set(value);
        }

        public string Output {
            get => Get<string>();
            private set => Set(value);
        }
    }
}