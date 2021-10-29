using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TypeCode.Business.Mode;
using TypeCode.Business.Mode.Specflow;
using TypeCode.Business.TypeEvaluation;
using TypeCode.Wpf.Helper.Commands;
using TypeCode.Wpf.Helper.ViewModel;

namespace TypeCode.Wpf.Specflow
{
    public class SpecflowViewModel : ViewModelBase
    {
        private readonly ITypeCodeGenerator<SpecflowTypeCodeGeneratorParameter> _specflowGenerator;
        private readonly ITypeProvider _typeProvider;
        private string _input;
        private string _output;

        public SpecflowViewModel(
            ITypeCodeGenerator<SpecflowTypeCodeGeneratorParameter> specflowGenerator,
            ITypeProvider typeProvider
        )
        {
            _specflowGenerator = specflowGenerator;
            _typeProvider = typeProvider;

            GenerateCommand = new AsyncRelayCommand(GenerateAsync);
        }

        private async Task GenerateAsync(object arg)
        {
            var inputNames = Input.Split(',').Select(name => name.Trim()).ToList();
            
            var parameter = new SpecflowTypeCodeGeneratorParameter
            {
                Types = _typeProvider.TryGetByNames(inputNames).ToList()
            };
            
            var result = await _specflowGenerator.GenerateAsync(parameter);
            Output = result;
        }
        
        public ICommand GenerateCommand { get; }

        public string Input
        {
            get => _input;
            set
            {
                _input = value;
                OnPropertyChanged();
            }
        }

        public string Output
        {
            get => _output;
            set
            {
                _output = value;
                OnPropertyChanged();
            }
        }
    }
}