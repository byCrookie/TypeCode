using System.Windows;
using System.Windows.Input;
using TypeCode.Business.Mode;
using TypeCode.Business.Mode.Mapper;
using TypeCode.Business.Mode.Mapper.Style;
using TypeCode.Business.TypeEvaluation;
using TypeCode.Wpf.Helper.Commands;
using TypeCode.Wpf.Helper.Navigation.Contract;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.ViewModel;
using TypeCode.Wpf.Pages.TypeSelection;

namespace TypeCode.Wpf.Pages.Mapper;

public class MapperViewModel : Reactive, IAsyncNavigatedTo
{
    private readonly ITypeCodeGenerator<MapperTypeCodeGeneratorParameter> _mapperGenerator;
    private readonly ITypeProvider _typeProvider;
    private readonly ITypeSelectionWizardStarter _typeSelectionWizardStarter;
    private MappingStyle _mappingStyle;

    public MapperViewModel(
        ITypeCodeGenerator<MapperTypeCodeGeneratorParameter> mapperGenerator,
        ITypeProvider typeProvider,
        ITypeSelectionWizardStarter typeSelectionWizardStarter
    )
    {
        _mapperGenerator = mapperGenerator;
        _typeProvider = typeProvider;
        _typeSelectionWizardStarter = typeSelectionWizardStarter;

        GenerateCommand = new AsyncRelayCommand(GenerateAsync);
        StyleCommand = new AsyncRelayCommand<MappingStyle>(StyleAsync);
        CopyToClipboardCommand = new AsyncRelayCommand(() =>
        {
            Clipboard.SetText(Output ?? string.Empty);
            return Task.CompletedTask;
        });
    }

    public Task OnNavigatedToAsync(NavigationContext context)
    {
        NewStyle = true;
        ExistingStyle = false;
        return Task.CompletedTask;
    }

    private Task StyleAsync(MappingStyle style)
    {
        _mappingStyle = style;
        return Task.CompletedTask;
    }

    private async Task GenerateAsync()
    {
        var inputNames = Input?.Split(',').Select(name => name.Trim()).ToList() ?? new List<string>();
        var types = _typeProvider.TryGetByName(inputNames.FirstOrDefault())
            .Union(_typeProvider.TryGetByName(inputNames.LastOrDefault()))
            .ToList();

        if (types.Any())
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

            var parameter = new MapperTypeCodeGeneratorParameter(
                new MappingType(types.FirstOrDefault()),
                new MappingType(types.LastOrDefault())
            )
            {
                MappingStyle = _mappingStyle,
                MapTree = MapTree,
                MapSingleDirectionOnly = MapSingleDirectionOnly,
            };

            var result = await _mapperGenerator.GenerateAsync(parameter).ConfigureAwait(true);
            Output = result;
        }
    }

    public ICommand GenerateCommand { get; set; }
    public ICommand StyleCommand { get; set; }
    public ICommand CopyToClipboardCommand { get; set; }

    public string? Input
    {
        get => Get<string?>();
        set => Set(value);
    }

    public string? Output
    {
        get => Get<string?>();
        private set => Set(value);
    }

    public bool NewStyle
    {
        get => Get<bool>();
        private set => Set(value);
    }

    public bool ExistingStyle
    {
        get => Get<bool>();
        private set => Set(value);
    }
    
    public bool MapTree
    {
        get => Get<bool>();
        set => Set(value);
    }
    
    public bool MapSingleDirectionOnly
    {
        get => Get<bool>();
        set => Set(value);
    }
}