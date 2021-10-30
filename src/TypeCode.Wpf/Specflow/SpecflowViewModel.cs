using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AsyncAwaitBestPractices.MVVM;
using TypeCode.Business.Mode;
using TypeCode.Business.Mode.Specflow;
using TypeCode.Business.TypeEvaluation;
using TypeCode.Wpf.Helper.ViewModel;

namespace TypeCode.Wpf.Specflow
{
    public class SpecflowViewModel : Reactive
    {
        private readonly ITypeCodeGenerator<SpecflowTypeCodeGeneratorParameter> _specflowGenerator;
        private readonly ITypeProvider _typeProvider;

        public SpecflowViewModel(
            ITypeCodeGenerator<SpecflowTypeCodeGeneratorParameter> specflowGenerator,
            ITypeProvider typeProvider
        )
        {
            _specflowGenerator = specflowGenerator;
            _typeProvider = typeProvider;

            GenerateCommand = new AsyncCommand(GenerateAsync);
        }

        private async Task GenerateAsync()
        {
            throw new Exception("Hello");
            
            var inputNames = Input.Split(',').Select(name => name.Trim()).ToList();
            
            var parameter = new SpecflowTypeCodeGeneratorParameter
            {
                Types = _typeProvider.TryGetByNames(inputNames).ToList()
            };
            
            var result = await _specflowGenerator.GenerateAsync(parameter);
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