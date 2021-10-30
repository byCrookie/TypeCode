using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TypeCode.Business.Mode;
using TypeCode.Business.Mode.UnitTestDependency.Manually;
using TypeCode.Business.Mode.UnitTestDependency.Type;
using TypeCode.Wpf.Helper.Commands;
using TypeCode.Wpf.Helper.ViewModel;

namespace TypeCode.Wpf.UnitTestDependencyManually
{
    public class UnitTestDependencyManuallyViewModel : Reactive
    {
        private readonly ITypeCodeGenerator<UnitTestDependencyManuallyGeneratorParameter> _unitTestDependencyManuallyGenerator;

        public UnitTestDependencyManuallyViewModel(
            ITypeCodeGenerator<UnitTestDependencyManuallyGeneratorParameter> unitTestDependencyManuallyGenerator
        )
        {
            _unitTestDependencyManuallyGenerator = unitTestDependencyManuallyGenerator;

            GenerateCommand = new AsyncRelayCommand(GenerateAsync);
        }

        private async Task GenerateAsync(object arg)
        {
            var parameter = new UnitTestDependencyManuallyGeneratorParameter
            {
                Input = Input
            };
            
            var result = await _unitTestDependencyManuallyGenerator.GenerateAsync(parameter);
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