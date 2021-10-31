using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AsyncAwaitBestPractices.MVVM;
using TypeCode.Business.Mode;
using TypeCode.Business.Mode.Builder;
using TypeCode.Business.TypeEvaluation;
using TypeCode.Wpf.Helper.Navigation;
using TypeCode.Wpf.Helper.Navigation.Contract;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.Navigation.Wizard.Service;
using TypeCode.Wpf.Helper.ViewModel;
using TypeCode.Wpf.Pages.Specflow;
using TypeCode.Wpf.Pages.TypeSelection;

namespace TypeCode.Wpf.Pages.Builder
{
    public class BuilderViewModel : Reactive, IAsyncNavigatedTo
    {
        private readonly ITypeCodeGenerator<BuilderTypeCodeGeneratorParameter> _builderGenerator;
        private readonly ITypeProvider _typeProvider;
        private readonly IWizardNavigationService _wizardNavigationService;

        public BuilderViewModel(
            ITypeCodeGenerator<BuilderTypeCodeGeneratorParameter> builderGenerator,
            ITypeProvider typeProvider,
            IWizardNavigationService wizardNavigationService
        )
        {
            _builderGenerator = builderGenerator;
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
            var navigationContext = new NavigationContext();
            navigationContext.AddParameter(new TypeSelectionParameter
            {
                AllowMultiSelection = false,
                Types =_typeProvider.TryGetByName(Input?.Trim())
            });
            
            var selectionViewModel = await _wizardNavigationService
                .OpenWizard(new WizardParameter<TypeSelectionViewModel>
                {
                    FinishButtonText = "Save"
                }, navigationContext);
            
            var parameter = new BuilderTypeCodeGeneratorParameter
            {
                Type = selectionViewModel.SelectedTypes.Single()
            };
            
            var result = await _builderGenerator.GenerateAsync(parameter);
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