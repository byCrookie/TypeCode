using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TypeCode.Business.Mode;
using TypeCode.Business.Mode.UnitTestDependency.Manually;
using TypeCode.Wpf.Components.OutputBox;

namespace TypeCode.Wpf.Pages.UnitTestDependencyManually;

public partial class UnitTestDependencyManuallyViewModel : ObservableObject
{
    private readonly ITypeCodeGenerator<UnitTestDependencyManuallyGeneratorParameter> _unitTestDependencyManuallyGenerator;

    public UnitTestDependencyManuallyViewModel(
        ITypeCodeGenerator<UnitTestDependencyManuallyGeneratorParameter> unitTestDependencyManuallyGenerator,
        IOutputBoxViewModelFactory outputBoxViewModelFactory
    )
    {
        _unitTestDependencyManuallyGenerator = unitTestDependencyManuallyGenerator;

        OutputBoxViewModel = outputBoxViewModelFactory.Create();
    }

    [RelayCommand]
    private async Task GenerateAsync()
    {
        var parameter = new UnitTestDependencyManuallyGeneratorParameter
        {
            Input = Input
        };

        var result = await _unitTestDependencyManuallyGenerator.GenerateAsync(parameter).ConfigureAwait(true);
        OutputBoxViewModel?.SetOutput(result);
    }

    [ObservableProperty]
    private string? _input;

    [ObservableProperty]
    private OutputBoxViewModel? _outputBoxViewModel;
}