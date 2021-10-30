using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AsyncAwaitBestPractices.MVVM;
using TypeCode.Business.Mode;
using TypeCode.Business.Mode.Composer;
using TypeCode.Business.Mode.UnitTestDependency.Type;
using TypeCode.Business.TypeEvaluation;
using TypeCode.Wpf.Helper.Navigation;
using TypeCode.Wpf.Helper.ViewModel;

namespace TypeCode.Wpf.Composer
{
    public class ComposerViewModel : Reactive, IAsyncNavigatedTo
    {
        private readonly ITypeCodeGenerator<ComposerTypeCodeGeneratorParameter> _composerTypeGenerator;
        private readonly ITypeProvider _typeProvider;

        public ComposerViewModel(
            ITypeCodeGenerator<ComposerTypeCodeGeneratorParameter> composerTypeGenerator,
            ITypeProvider typeProvider
        )
        {
            _composerTypeGenerator = composerTypeGenerator;
            _typeProvider = typeProvider;
        }
        
        public Task OnNavigatedToAsync(NavigationContext context)
        {
            GenerateCommand = new AsyncCommand(GenerateAsync);
            return Task.CompletedTask;
        }

        private async Task GenerateAsync()
        {
            var type = _typeProvider.TryGetByName(Input).FirstOrDefault();

            var interfaceTypes = _typeProvider
                .TryGetTypesByCondition(typ => typ.GetInterface(type?.Name ?? string.Empty) != null)
                .ToList();

            var parameter = new ComposerTypeCodeGeneratorParameter
            {
                Type = type,
                Interfaces = interfaceTypes
            };
            
            var result = await _composerTypeGenerator.GenerateAsync(parameter);
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