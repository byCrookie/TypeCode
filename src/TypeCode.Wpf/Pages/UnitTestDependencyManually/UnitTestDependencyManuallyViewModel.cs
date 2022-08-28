using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TypeCode.Business.Mode;
using TypeCode.Business.Mode.UnitTestDependency.Manually;
using TypeCode.Wpf.Components.OutputBox;
using TypeCode.Wpf.Helper.ViewModels;

namespace TypeCode.Wpf.Pages.UnitTestDependencyManually;

public partial class UnitTestDependencyManuallyViewModel : ViewModelBase
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

    [RelayCommand(CanExecute = nameof(CanGenerate))]
    private async Task GenerateAsync()
    {
        var parameter = new UnitTestDependencyManuallyGeneratorParameter
        {
            Input = Input
        };

        var result = await _unitTestDependencyManuallyGenerator.GenerateAsync(parameter).ConfigureAwait(true);
        OutputBoxViewModel?.SetOutput(result);
    }
    
    private bool CanGenerate()
    {
        return !HasErrors && !string.IsNullOrEmpty(Input);
    }

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [NotifyCanExecuteChangedFor(nameof(GenerateCommand))]
    [Required]
    private string? _input;

    [ObservableProperty]
    [ChildViewModel]
    private OutputBoxViewModel? _outputBoxViewModel;
}