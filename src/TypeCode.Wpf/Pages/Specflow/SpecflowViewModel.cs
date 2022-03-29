using TypeCode.Business.Mode;
using TypeCode.Business.Mode.Specflow;
using TypeCode.Business.TypeEvaluation;
using TypeCode.Wpf.Components.InputBox;
using TypeCode.Wpf.Components.OutputBox;
using TypeCode.Wpf.Helper.Navigation.Contract;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.ViewModel;
using TypeCode.Wpf.Pages.TypeSelection;

namespace TypeCode.Wpf.Pages.Specflow;

public class SpecflowViewModel : Reactive, IAsyncNavigatedTo
{
    private readonly ITypeCodeGenerator<SpecflowTypeCodeGeneratorParameter> _specflowGenerator;
    private readonly ITypeProvider _typeProvider;
    private readonly ITypeSelectionWizardStarter _typeSelectionWizardStarter;

    public SpecflowViewModel(
        ITypeCodeGenerator<SpecflowTypeCodeGeneratorParameter> specflowGenerator,
        ITypeProvider typeProvider,
        ITypeSelectionWizardStarter typeSelectionWizardStarter,
        IInputBoxViewModelFactory inputBoxViewModelFactory,
        IOutputBoxViewModelFactory outputBoxViewModelFactory
    )
    {
        _specflowGenerator = specflowGenerator;
        _typeProvider = typeProvider;
        _typeSelectionWizardStarter = typeSelectionWizardStarter;

        IncludeStrings = true;

        var parameter = new InputBoxViewModelParameter("Generate", GenerateAsync)
        {
            ToolTip = "Input type name. Multiple type names can be seperated by using ','."
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
        var inputNames = input?.Split(',').Select(name => name.Trim()).ToList() ?? new List<string>();
        var types = _typeProvider.TryGetByNames(inputNames, new TypeEvaluationOptions { Regex = regex }).ToList();

        if (types.Count > 1)
        {
            var typeSelectionParameter = new TypeSelectionParameter
            {
                AllowMultiSelection = true,
                Types = types
            };

            await _typeSelectionWizardStarter.StartAsync(typeSelectionParameter, selectedTypes =>
            {
                types = selectedTypes.ToList();
                return Task.CompletedTask;
            }, _ =>
            {
                types = new List<Type>();
                return Task.CompletedTask;
            }).ConfigureAwait(true);
        }

        var parameter = new SpecflowTypeCodeGeneratorParameter
        {
            Types = types,
            OnlyRequired = OnlyRequired,
            SortAlphabetically = SortAlphabetically,
            IncludeStrings = IncludeStrings
        };

        var result = await _specflowGenerator.GenerateAsync(parameter).ConfigureAwait(true);
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

    public bool IncludeStrings
    {
        get => Get<bool>();
        set => Set(value);
    }

    public bool OnlyRequired
    {
        get => Get<bool>();
        set => Set(value);
    }

    public bool SortAlphabetically
    {
        get => Get<bool>();
        set => Set(value);
    }
}