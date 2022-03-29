using System.Windows;
using System.Windows.Input;
using TypeCode.Business.Mode;
using TypeCode.Business.Mode.Composer;
using TypeCode.Business.TypeEvaluation;
using TypeCode.Wpf.Components.InputBox;
using TypeCode.Wpf.Helper.Commands;
using TypeCode.Wpf.Helper.Navigation.Contract;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.ViewModel;
using TypeCode.Wpf.Pages.TypeSelection;

namespace TypeCode.Wpf.Pages.Composer;

public class ComposerViewModel : Reactive, IAsyncNavigatedTo
{
    private readonly ITypeCodeGenerator<ComposerTypeCodeGeneratorParameter> _composerTypeGenerator;
    private readonly ITypeProvider _typeProvider;
    private readonly ITypeSelectionWizardStarter _typeSelectionWizardStarter;

    public ComposerViewModel(
        ITypeCodeGenerator<ComposerTypeCodeGeneratorParameter> composerTypeGenerator,
        ITypeProvider typeProvider,
        ITypeSelectionWizardStarter typeSelectionWizardStarter,
        IInputBoxViewModelFactory inputBoxViewModelFactory
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
        
        CopyToClipboardCommand = new AsyncRelayCommand(() =>
        {
            Clipboard.SetText(Output ?? string.Empty);
            return Task.CompletedTask;
        });
    }

    public Task OnNavigatedToAsync(NavigationContext context)
    {
        return Task.CompletedTask;
    }

    private async Task GenerateAsync(bool regex, string? input)
    {
        var types = _typeProvider.TryGetByName(input?.Trim(), new TypeEvaluationOptions{ Regex = regex }).ToList();
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

        if (selectedType is not null)
        {
            var parameter = new ComposerTypeCodeGeneratorParameter();
            parameter.ComposerTypes.Add(new ComposerType(
                selectedType,
                _typeProvider.TryGetTypesByCondition(typ => typ.GetInterface(selectedType.Name) != null).ToList()
            ));
            var result = await _composerTypeGenerator.GenerateAsync(parameter).ConfigureAwait(true);
            Output = result;
        }
        else
        {
            Output = null; 
        }
    }
    
    public ICommand CopyToClipboardCommand { get; set; }
    
    public InputBoxViewModel? InputBoxViewModel
    {
        get => Get<InputBoxViewModel?>();
        set => Set(value);
    }

    public string? Output
    {
        get => Get<string?>();
        private set => Set(value);
    }
}