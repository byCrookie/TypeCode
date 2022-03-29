using TypeCode.Business.Mode;
using TypeCode.Business.Mode.Builder;
using TypeCode.Business.TypeEvaluation;
using TypeCode.Wpf.Components.InputBox;
using TypeCode.Wpf.Components.OutputBox;
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
        ITypeSelectionWizardStarter typeSelectionWizardStarter,
        IInputBoxViewModelFactory inputBoxViewModelFactory,
        IOutputBoxViewModelFactory outputBoxViewModelFactory
    )
    {
        _builderGenerator = builderGenerator;
        _typeProvider = typeProvider;
        _typeSelectionWizardStarter = typeSelectionWizardStarter;

        var parameter = new InputBoxViewModelParameter("Generate", GenerateAsync)
        {
            ToolTip = "Input type name."
        };

        InputBoxViewModel = inputBoxViewModelFactory.Create(parameter);
        OutputBoxViewModel = outputBoxViewModelFactory.Create();
    }

    public Task OnNavigatedToAsync(NavigationContext context)
    {
        return Task.CompletedTask;
    }

    private async Task GenerateAsync(bool regex, string? input)
    {
        var types = _typeProvider.TryGetByName(input?.Trim(), new TypeEvaluationOptions { Regex = regex }).ToList();
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
            Type = selectedType,
            Recursive = Recursive
        };

        var result = await _builderGenerator.GenerateAsync(parameter).ConfigureAwait(true);
        OutputBoxViewModel?.SetOutput(result);
    }

    public InputBoxViewModel? InputBoxViewModel
    {
        get => Get<InputBoxViewModel?>();
        set => Set(value);
    }
    
    public OutputBoxViewModel? OutputBoxViewModel
    {
        get => Get<OutputBoxViewModel?>();
        set => Set(value);
    }

    public bool Recursive
    {
        get => Get<bool>();
        set => Set(value);
    }
}