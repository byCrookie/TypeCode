using CommunityToolkit.Mvvm.ComponentModel;
using TypeCode.Business.Mode;
using TypeCode.Business.Mode.Builder;
using TypeCode.Business.TypeEvaluation;
using TypeCode.Wpf.Components.InputBox;
using TypeCode.Wpf.Components.OutputBox;
using TypeCode.Wpf.Helper.ViewModels;
using TypeCode.Wpf.Pages.TypeSelection;

namespace TypeCode.Wpf.Pages.Builder;

public partial class BuilderViewModel : ObservableObject
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

    private async Task GenerateAsync(bool regex, string? input)
    {
        var types = _typeProvider.TryGetByName(input?.Trim(), new TypeEvaluationOptions { Regex = regex }).ToList();

        if (types.Count > 1)
        {
            var typeSelectionParameter = new TypeSelectionParameter
            {
                AllowMultiSelection = false,
                Types = types
            };

            await _typeSelectionWizardStarter.StartAsync(typeSelectionParameter, async viewModel =>
            {
                var selectedType = viewModel.SelectedTypes.Single();
                await GenerateAsync(selectedType).ConfigureAwait(true);
            }).ConfigureAwait(true);
        }
        else
        {
            await GenerateAsync(types.FirstOrDefault()).ConfigureAwait(true);
        }
    }

    private async Task GenerateAsync(Type? selectedType)
    {
        var parameter = new BuilderTypeCodeGeneratorParameter
        {
            Type = selectedType,
            Recursive = Recursive
        };

        var result = await _builderGenerator.GenerateAsync(parameter).ConfigureAwait(true);
        OutputBoxViewModel?.SetOutput(result);
    }

    [ObservableProperty]
    [ChildViewModel]
    private InputBoxViewModel? _inputBoxViewModel;

    [ObservableProperty]
    [ChildViewModel]
    private OutputBoxViewModel? _outputBoxViewModel;

    [ObservableProperty]
    private bool _recursive;
}