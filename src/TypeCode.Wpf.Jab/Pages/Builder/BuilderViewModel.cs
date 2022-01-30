using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AsyncAwaitBestPractices.MVVM;
using TypeCode.Business.Mode;
using TypeCode.Business.Mode.Builder;
using TypeCode.Business.TypeEvaluation;
using TypeCode.Wpf.Jab.Helper.Navigation.Contract;
using TypeCode.Wpf.Jab.Helper.Navigation.Service;
using TypeCode.Wpf.Jab.Helper.Navigation.Wizard.WizardSimple;
using TypeCode.Wpf.Jab.Helper.ViewModel;
using TypeCode.Wpf.Jab.Pages.TypeSelection;

namespace TypeCode.Wpf.Jab.Pages.Builder;

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
        var types = _typeProvider.TryGetByName(Input?.Trim()).ToList();
        var selectedType = types.FirstOrDefault();

        if (types.Count > 1)
        {
            var navigationContext = new NavigationContext();
            navigationContext.AddParameter(new TypeSelectionParameter
            {
                AllowMultiSelection = false,
                Types = types
            });
            
            var selectionViewModel = await _wizardNavigationService
                .OpenWizardAsync(new WizardParameter<TypeSelectionViewModel>
                {
                    FinishButtonText = "Select"
                }, navigationContext).ConfigureAwait(true);

            selectedType = selectionViewModel.SelectedTypes.Single();
        }

        var parameter = new BuilderTypeCodeGeneratorParameter
        {
            Type = selectedType
        };
            
        var result = await _builderGenerator.GenerateAsync(parameter).ConfigureAwait(true);
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