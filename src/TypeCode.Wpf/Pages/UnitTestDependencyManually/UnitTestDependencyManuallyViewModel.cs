using System.Windows;
using System.Windows.Input;
using TypeCode.Business.Mode;
using TypeCode.Business.Mode.UnitTestDependency.Manually;
using TypeCode.Wpf.Helper.Commands;
using TypeCode.Wpf.Helper.Navigation.Contract;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.ViewModel;

namespace TypeCode.Wpf.Pages.UnitTestDependencyManually;

public class UnitTestDependencyManuallyViewModel : Reactive, IAsyncNavigatedTo
{
    private readonly ITypeCodeGenerator<UnitTestDependencyManuallyGeneratorParameter> _unitTestDependencyManuallyGenerator;

    public UnitTestDependencyManuallyViewModel(
        ITypeCodeGenerator<UnitTestDependencyManuallyGeneratorParameter> unitTestDependencyManuallyGenerator
    )
    {
        _unitTestDependencyManuallyGenerator = unitTestDependencyManuallyGenerator;
        
        GenerateCommand = new AsyncRelayCommand(GenerateAsync);
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

    private async Task GenerateAsync()
    {
        var parameter = new UnitTestDependencyManuallyGeneratorParameter
        {
            Input = Input
        };
            
        var result = await _unitTestDependencyManuallyGenerator.GenerateAsync(parameter).ConfigureAwait(true);
        Output = result;
    }
        
    public ICommand GenerateCommand { get; set; }
    public ICommand CopyToClipboardCommand { get; set; }

    public string? Input {
        get => Get<string?>();
        set => Set(value);
    }

    public string? Output {
        get => Get<string?>();
        private set => Set(value);
    }
}