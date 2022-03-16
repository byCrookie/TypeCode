using System.Windows.Input;
using AsyncAwaitBestPractices.MVVM;
using TypeCode.Business.Mode;
using TypeCode.Business.Mode.Builder;
using TypeCode.Business.TypeEvaluation;
using TypeCode.Wpf.Helper.Navigation.Contract;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.ViewModel;
using TypeCode.Wpf.Pages.TypeSelection;

namespace TypeCode.Wpf.Pages.Builder;

public class BuilderViewModel : Reactive, IAsyncNavigatedTo
{
    private readonly ITypeCodeGenerator<BuilderTypeCodeGeneratorParameter> _builderGenerator;
    private readonly ITypeProvider _typeProvider;
    private readonly ITypeSelectionWizardStarter _typeSelectionWizardStarter;

    public BuilderViewModel(
        ITypeCodeGenerator<BuilderTypeCodeGeneratorParameter> builderGenerator,
        ITypeProvider typeProvider,
        ITypeSelectionWizardStarter typeSelectionWizardStarter
    )
    {
        _builderGenerator = builderGenerator;
        _typeProvider = typeProvider;
        _typeSelectionWizardStarter = typeSelectionWizardStarter;

        GenerateCommand = new AsyncCommand(GenerateAsync);
    }
        
    public Task OnNavigatedToAsync(NavigationContext context)
    {
        return Task.CompletedTask;
    }

    private async Task GenerateAsync()
    {
        var types = _typeProvider.TryGetByName(Input?.Trim()).ToList();
        var selectedType = types.FirstOrDefault();

        if (types.Count > 1)
        {
            var typeSelectionParameter = new TypeSelectionParameter
            {
                AllowMultiSelection = false,
                Types = types
            };

            await _typeSelectionWizardStarter.StartAsync(typeSelectionParameter, selectedTypes =>
            {
                selectedType = selectedTypes.Single();
                return Task.CompletedTask;
            }, _ =>
            {
                selectedType = null;
                return Task.CompletedTask;
            }).ConfigureAwait(true);
        }

        var parameter = new BuilderTypeCodeGeneratorParameter
        {
            Types = selectedType is not null ? new List<Type>{selectedType} : new List<Type>()
        };
            
        var result = await _builderGenerator.GenerateAsync(parameter).ConfigureAwait(true);
        Output = result;
    }
        
    public ICommand GenerateCommand { get; set; }
        
    public string? Input {
        get => Get<string?>();
        set => Set(value);
    }

    public string? Output {
        get => Get<string?>();
        private set => Set(value);
    }
}