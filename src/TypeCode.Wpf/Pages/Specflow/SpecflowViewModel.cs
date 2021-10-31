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
using TypeCode.Wpf.Helper.Navigation.Wizard.Service;
using TypeCode.Wpf.Helper.ViewModel;
using TypeCode.Wpf.Pages.TypeSelection;

namespace TypeCode.Wpf.Pages.Specflow
{
    public class SpecflowViewModel : Reactive, IAsyncNavigatedTo
    {
        private readonly ITypeCodeGenerator<SpecflowTypeCodeGeneratorParameter> _specflowGenerator;
        private readonly ITypeProvider _typeProvider;
        private readonly IWizardNavigationService _wizardNavigationService;

        public SpecflowViewModel(
            ITypeCodeGenerator<SpecflowTypeCodeGeneratorParameter> specflowGenerator,
            ITypeProvider typeProvider,
            IWizardNavigationService wizardNavigationService
        )
        {
            _specflowGenerator = specflowGenerator;
            _typeProvider = typeProvider;
            _wizardNavigationService = wizardNavigationService;
        }

        public Task OnNavigatedToAsync(NavigationContext context)
        {
            GenerateCommand = new AsyncCommand(GenerateAsync);
            return Task.CompletedTask;
        }

        private async Task GenerateAsync()
        {
            var inputNames = Input?.Split(',').Select(name => name.Trim()).ToList() ?? new List<string>();
            var types = _typeProvider.TryGetByNames(inputNames).ToList();

            if (types.Count > 1)
            {
                var navigationContext = new NavigationContext();
                navigationContext.AddParameter(new TypeSelectionParameter
                {
                    AllowMultiSelection = true,
                    Types = types
                });

                var selectionViewModel = await _wizardNavigationService
                    .OpenWizard(new WizardParameter<TypeSelectionViewModel>
                    {
                        FinishButtonText = "Select"
                    }, navigationContext);

                types = selectionViewModel.SelectedTypes.ToList();
            }
            
            var parameter = new SpecflowTypeCodeGeneratorParameter
            {
                Types = types
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