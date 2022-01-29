using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AsyncAwaitBestPractices.MVVM;
using TypeCode.Business.Mode;
using TypeCode.Business.Mode.Composer;
using TypeCode.Business.TypeEvaluation;
using TypeCode.Wpf.Helper.Navigation.Contract;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.Navigation.Wizard.WizardSimple;
using TypeCode.Wpf.Helper.ViewModel;
using TypeCode.Wpf.Pages.TypeSelection;

namespace TypeCode.Wpf.Pages.Composer
{
    public class ComposerViewModel : Reactive, IAsyncNavigatedTo
    {
        private readonly ITypeCodeGenerator<ComposerTypeCodeGeneratorParameter> _composerTypeGenerator;
        private readonly ITypeProvider _typeProvider;
        private readonly IWizardNavigationService _wizardNavigationService;

        public ComposerViewModel(
            ITypeCodeGenerator<ComposerTypeCodeGeneratorParameter> composerTypeGenerator,
            ITypeProvider typeProvider,
            IWizardNavigationService wizardNavigationService
        )
        {
            _composerTypeGenerator = composerTypeGenerator;
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
            var types = _typeProvider.TryGetByName(Input?.Trim()).ToList();
            var selectedType = types.FirstOrDefault();

            if (types.Count > 1)
            {
                var navigationContext = new NavigationContext();
                navigationContext.AddParameter(new TypeSelectionParameter
                {
                    AllowMultiSelection = false,
                    Types =types
                });
            
                var selectionViewModel = await _wizardNavigationService
                    .OpenWizardAsync(new WizardParameter<TypeSelectionViewModel>
                    {
                        FinishButtonText = "Select"
                    }, navigationContext).ConfigureAwait(true);

                selectedType = selectionViewModel.SelectedTypes.Single();
            }
            
            var parameter = new ComposerTypeCodeGeneratorParameter
            {
                Type = selectedType,
                Interfaces = _typeProvider
                    .TryGetTypesByCondition(typ => typ.GetInterface(selectedType?.Name ?? string.Empty) != null)
                    .ToList()
            };
            
            var result = await _composerTypeGenerator.GenerateAsync(parameter).ConfigureAwait(true);
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