using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AsyncAwaitBestPractices.MVVM;
using TypeCode.Business.Mode;
using TypeCode.Business.Mode.UnitTestDependency.Type;
using TypeCode.Business.TypeEvaluation;
using TypeCode.Wpf.Helper.Navigation.Contract;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.Navigation.Wizard.WizardSimple;
using TypeCode.Wpf.Helper.ViewModel;
using TypeCode.Wpf.Pages.TypeSelection;

namespace TypeCode.Wpf.Pages.UnitTestDependencyType
{
    public class UnitTestDependencyTypeViewModel : Reactive, IAsyncNavigatedTo
    {
        private readonly ITypeCodeGenerator<UnitTestDependencyTypeGeneratorParameter> _unitTestDependencyTypeGenerator;
        private readonly ITypeProvider _typeProvider;
        private readonly IWizardNavigationService _wizardNavigationService;

        public UnitTestDependencyTypeViewModel(
            ITypeCodeGenerator<UnitTestDependencyTypeGeneratorParameter> unitTestDependencyTypeGenerator,
            ITypeProvider typeProvider,
            IWizardNavigationService wizardNavigationService
        )
        {
            _unitTestDependencyTypeGenerator = unitTestDependencyTypeGenerator;
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
                    .OpenWizardAsync(new WizardParameter<TypeSelectionViewModel>
                    {
                        FinishButtonText = "Select"
                    }, navigationContext).ConfigureAwait(true);

                types = selectionViewModel.SelectedTypes.ToList();
            }
            
            var parameter = new UnitTestDependencyTypeGeneratorParameter
            {
                Types = types
            };
            
            var result = await _unitTestDependencyTypeGenerator.GenerateAsync(parameter).ConfigureAwait(true);
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