using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AsyncAwaitBestPractices.MVVM;
using TypeCode.Business.Mode;
using TypeCode.Business.Mode.Specflow;
using TypeCode.Business.TypeEvaluation;
using TypeCode.Wpf.Helper.Navigation;
using TypeCode.Wpf.Helper.Navigation.Contract;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.ViewModel;

namespace TypeCode.Wpf.Pages.Specflow
{
    public class SpecflowViewModel : Reactive, IAsyncNavigatedTo
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
        }

        public Task OnNavigatedToAsync(NavigationContext context)
        {
            GenerateCommand = new AsyncCommand(GenerateAsync);
            return Task.CompletedTask;
        }

        private async Task GenerateAsync()
        {
            var inputNames = Input?.Split(',').Select(name => name.Trim()).ToList() ?? new List<string>();

            var parameter = new SpecflowTypeCodeGeneratorParameter
            {
                Types = _typeProvider.TryGetByNames(inputNames).ToList()
            };

            var result = await _specflowGenerator.GenerateAsync(parameter);
            Output = result;
        }

        public ICommand GenerateCommand { get; set; }

        public string Input
        {
            get => Get<string>();
            set => Set(value);
        }

        public string Output
        {
            get => Get<string>();
            private set => Set(value);
        }
    }
}