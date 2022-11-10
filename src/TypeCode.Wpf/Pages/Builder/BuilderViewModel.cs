using CommunityToolkit.Mvvm.ComponentModel;
using TypeCode.Business.Mode;
using TypeCode.Business.Mode.Builder;
using TypeCode.Business.TypeEvaluation;
using TypeCode.Wpf.Components.InputBox;
using TypeCode.Wpf.Components.OutputBox;
using TypeCode.Wpf.Helper.Navigation.Contract;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.ViewModels;
using TypeCode.Wpf.Pages.TypeSelection;

namespace TypeCode.Wpf.Pages.Builder;

public sealed partial class BuilderViewModel : ViewModelBase, IAsyncInitialNavigated
{
    private readonly ITypeCodeGenerator<BuilderTypeCodeGeneratorParameter> _builderGenerator;
    private readonly ILazyTypeProviderFactory _lazyTypeProviderFactory;
    private readonly ITypeSelectionWizardStarter _typeSelectionWizardStarter;
    private readonly IInputBoxViewModelFactory _inputBoxViewModelFactory;
    private readonly IOutputBoxViewModelFactory _outputBoxViewModelFactory;

    public BuilderViewModel(
        ITypeCodeGenerator<BuilderTypeCodeGeneratorParameter> builderGenerator,
        ILazyTypeProviderFactory lazyTypeProviderFactory,
        ITypeSelectionWizardStarter typeSelectionWizardStarter,
        IInputBoxViewModelFactory inputBoxViewModelFactory,
        IOutputBoxViewModelFactory outputBoxViewModelFactory
    )
    {
        _builderGenerator = builderGenerator;
        _lazyTypeProviderFactory = lazyTypeProviderFactory;
        _typeSelectionWizardStarter = typeSelectionWizardStarter;
        _inputBoxViewModelFactory = inputBoxViewModelFactory;
        _outputBoxViewModelFactory = outputBoxViewModelFactory;
    }
    
    public Task OnInititalNavigationAsync(NavigationContext context)
    {
        var parameter = new InputBoxViewModelParameter("Generate", GenerateAsync)
        {
            ToolTip = "Input type name."
        };

        InputBoxViewModel = _inputBoxViewModelFactory.Create(parameter);
        OutputBoxViewModel = _outputBoxViewModelFactory.Create();
        return Task.CompletedTask;
    }

    private async Task GenerateAsync(bool regex, string? input)
    {
        var typeProvider = await _lazyTypeProviderFactory.ValueAsync().ConfigureAwait(false);
        var types = typeProvider.TryGetByName(input?.Trim(), new TypeEvaluationOptions { Regex = regex }).ToList();

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