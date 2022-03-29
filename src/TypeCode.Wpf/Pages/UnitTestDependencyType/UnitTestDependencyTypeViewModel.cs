using System.Windows;
using System.Windows.Input;
using TypeCode.Business.Mode;
using TypeCode.Business.Mode.UnitTestDependency.Type;
using TypeCode.Business.TypeEvaluation;
using TypeCode.Wpf.Components.InputBox;
using TypeCode.Wpf.Helper.Commands;
using TypeCode.Wpf.Helper.Navigation.Contract;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.ViewModel;
using TypeCode.Wpf.Pages.TypeSelection;

namespace TypeCode.Wpf.Pages.UnitTestDependencyType;

public class UnitTestDependencyTypeViewModel : Reactive, IAsyncNavigatedTo
{
    private readonly ITypeCodeGenerator<UnitTestDependencyTypeGeneratorParameter> _unitTestDependencyTypeGenerator;
    private readonly ITypeProvider _typeProvider;
    private readonly ITypeSelectionWizardStarter _typeSelectionWizardStarter;

    public UnitTestDependencyTypeViewModel(
        ITypeCodeGenerator<UnitTestDependencyTypeGeneratorParameter> unitTestDependencyTypeGenerator,
        ITypeProvider typeProvider,
        ITypeSelectionWizardStarter typeSelectionWizardStarter,
        IInputBoxViewModelFactory inputBoxViewModelFactory
    )
    {
        _unitTestDependencyTypeGenerator = unitTestDependencyTypeGenerator;
        _typeProvider = typeProvider;
        _typeSelectionWizardStarter = typeSelectionWizardStarter;

        var parameter = new InputBoxViewModelParameter("Generate", GenerateAsync)
        {
            ToolTip = "Input type name. Multiple type names can be seperated by using ','."
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

        var parameter = new UnitTestDependencyTypeGeneratorParameter
        {
            Types = types
        };

        var result = await _unitTestDependencyTypeGenerator.GenerateAsync(parameter).ConfigureAwait(true);
        Output = result;
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