using CommunityToolkit.Mvvm.ComponentModel;
using TypeCode.Business.Mode;
using TypeCode.Business.Mode.Composer;
using TypeCode.Business.TypeEvaluation;
using TypeCode.Wpf.Components.InputBox;
using TypeCode.Wpf.Components.OutputBox;
using TypeCode.Wpf.Helper.ViewModels;
using TypeCode.Wpf.Pages.TypeSelection;

namespace TypeCode.Wpf.Pages.Composer;

public partial class ComposerViewModel : ViewModelBase
{
    private readonly ITypeCodeGenerator<ComposerTypeCodeGeneratorParameter> _composerTypeGenerator;
    private readonly ITypeProvider _typeProvider;
    private readonly ITypeSelectionWizardStarter _typeSelectionWizardStarter;

    public ComposerViewModel(
        ITypeCodeGenerator<ComposerTypeCodeGeneratorParameter> composerTypeGenerator,
        ITypeProvider typeProvider,
        ITypeSelectionWizardStarter typeSelectionWizardStarter,
        IInputBoxViewModelFactory inputBoxViewModelFactory,
        IOutputBoxViewModelFactory outputBoxViewModelFactory
    )
    {
        _composerTypeGenerator = composerTypeGenerator;
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
        var types = _typeProvider.TryGetByName(input?.Trim(), new TypeEvaluationOptions{ Regex = regex }).ToList();

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
        if (selectedType is not null)
        {
            var parameter = new ComposerTypeCodeGeneratorParameter();
            parameter.ComposerTypes.Add(new ComposerType(
                selectedType,
                _typeProvider.TryGetTypesByCondition(typ => typ.GetInterface(selectedType.Name) != null).ToList()
            ));
            var result = await _composerTypeGenerator.GenerateAsync(parameter).ConfigureAwait(true);
            OutputBoxViewModel?.SetOutput(result);
        }
    }

    [ObservableProperty]
    [ChildViewModel]
    private InputBoxViewModel? _inputBoxViewModel;

    [ObservableProperty]
    [ChildViewModel]
    private OutputBoxViewModel? _outputBoxViewModel;
}