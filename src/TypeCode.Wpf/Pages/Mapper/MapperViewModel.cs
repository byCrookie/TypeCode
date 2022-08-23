using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TypeCode.Business.Mode;
using TypeCode.Business.Mode.Mapper;
using TypeCode.Business.Mode.Mapper.Style;
using TypeCode.Business.TypeEvaluation;
using TypeCode.Wpf.Components.InputBox;
using TypeCode.Wpf.Components.OutputBox;
using TypeCode.Wpf.Helper.Navigation.Contract;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.ViewModels;
using TypeCode.Wpf.Pages.TypeSelection;

namespace TypeCode.Wpf.Pages.Mapper;

public partial class MapperViewModel : ObservableObject, IAsyncNavigatedTo
{
    private readonly ITypeCodeGenerator<MapperTypeCodeGeneratorParameter> _mapperGenerator;
    private readonly ITypeProvider _typeProvider;
    private readonly ITypeSelectionWizardStarter _typeSelectionWizardStarter;
    private MappingStyle _mappingStyle;

    public MapperViewModel(
        ITypeCodeGenerator<MapperTypeCodeGeneratorParameter> mapperGenerator,
        ITypeProvider typeProvider,
        ITypeSelectionWizardStarter typeSelectionWizardStarter,
        IInputBoxViewModelFactory inputBoxViewModelFactory,
        IOutputBoxViewModelFactory outputBoxViewModelFactory
    )
    {
        _mapperGenerator = mapperGenerator;
        _typeProvider = typeProvider;
        _typeSelectionWizardStarter = typeSelectionWizardStarter;

        var parameter = new InputBoxViewModelParameter("Generate", GenerateAsync)
        {
            ToolTip = "Input two type names seperated by ','."
        };

        InputBoxViewModel = inputBoxViewModelFactory.Create(parameter);
        OutputBoxViewModel = outputBoxViewModelFactory.Create();
    }

    public Task OnNavigatedToAsync(NavigationContext context)
    {
        NewStyle = true;
        ExistingStyle = false;
        return Task.CompletedTask;
    }

    [RelayCommand]
    private Task StyleAsync(MappingStyle style)
    {
        _mappingStyle = style;
        return Task.CompletedTask;
    }

    private async Task GenerateAsync(bool regex, string? input)
    {
        var inputNames = input?.Split(',').Select(name => name.Trim()).ToList() ?? new List<string>();
        var types = _typeProvider.TryGetByName(inputNames.FirstOrDefault(), new TypeEvaluationOptions { Regex = regex })
            .Union(_typeProvider.TryGetByName(inputNames.LastOrDefault(), new TypeEvaluationOptions { Regex = regex }))
            .ToList();

        if (types.Any())
        {
            var typeSelectionParameter = new TypeSelectionParameter
            {
                AllowMultiSelection = true,
                Types = types
            };

            await _typeSelectionWizardStarter.StartAsync(typeSelectionParameter, async viewModel =>
            {
                var selectedTypes = viewModel.SelectedTypes.ToList();

                var parameter = new MapperTypeCodeGeneratorParameter(
                    new MappingType(selectedTypes.FirstOrDefault()),
                    new MappingType(selectedTypes.LastOrDefault())
                )
                {
                    MappingStyle = _mappingStyle,
                    Recursiv = Recursiv,
                    SingleDirectionOnly = SingleDirectionOnly
                };

                var result = await _mapperGenerator.GenerateAsync(parameter).ConfigureAwait(true);
                OutputBoxViewModel?.SetOutput(result);
            }).ConfigureAwait(true);
        }
    }

    [ObservableProperty]
    [ChildViewModel]
    private InputBoxViewModel? _inputBoxViewModel;

    [ObservableProperty]
    [ChildViewModel]
    private OutputBoxViewModel? _outputBoxViewModel;

    [ObservableProperty]
    private bool _newStyle;

    [ObservableProperty]
    private bool _existingStyle;

    [ObservableProperty]
    private bool _recursiv;

    [ObservableProperty]
    private bool _singleDirectionOnly;
}