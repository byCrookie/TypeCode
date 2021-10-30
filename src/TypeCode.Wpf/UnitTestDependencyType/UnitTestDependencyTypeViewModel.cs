using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AsyncAwaitBestPractices.MVVM;
using TypeCode.Business.Mode;
using TypeCode.Business.Mode.UnitTestDependency.Type;
using TypeCode.Business.TypeEvaluation;
using TypeCode.Wpf.Helper.Navigation;
using TypeCode.Wpf.Helper.ViewModel;

namespace TypeCode.Wpf.UnitTestDependencyType
{
    public class UnitTestDependencyTypeViewModel : Reactive, IAsyncNavigatedTo
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
        }
        
        public Task OnNavigatedToAsync(NavigationContext context)
        {
            GenerateCommand = new AsyncCommand(GenerateAsync);
            return Task.CompletedTask;
        }

        private async Task GenerateAsync()
        {
            var inputNames = Input?.Split(',').Select(name => name.Trim()).ToList() ?? new List<string>();
            
            var parameter = new UnitTestDependencyTypeGeneratorParameter
            {
                Types = _typeProvider.TryGetByNames(inputNames).ToList()
            };
            
            var result = await _unitTestDependencyTypeGenerator.GenerateAsync(parameter);
            Output = result;
        }
        
        public ICommand GenerateCommand { get; set; }
        
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