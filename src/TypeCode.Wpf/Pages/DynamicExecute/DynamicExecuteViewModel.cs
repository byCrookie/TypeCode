using System.IO;
using System.Text;
using System.Windows.Input;
using TypeCode.Business.Mode;
using TypeCode.Business.Mode.UnitTestDependency.Manually;
using TypeCode.Wpf.Components.OutputBox;
using TypeCode.Wpf.Helper.Commands;
using TypeCode.Wpf.Helper.Navigation.Contract;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.ViewModel;
using TypeCode.Wpf.Pages.DynamicExecute.Code;

namespace TypeCode.Wpf.Pages.DynamicExecute;

public class DynamicExecuteViewModel : Reactive, IAsyncNavigatedTo
{
    private readonly ITypeCodeGenerator<UnitTestDependencyManuallyGeneratorParameter> _unitTestDependencyManuallyGenerator;
    private readonly ICompiler _compiler;
    private readonly IRunner _runner;

    public DynamicExecuteViewModel(
        ITypeCodeGenerator<UnitTestDependencyManuallyGeneratorParameter> unitTestDependencyManuallyGenerator,
        IOutputBoxViewModelFactory outputBoxViewModelFactory,
        ICompiler compiler,
        IRunner runner
    )
    {
        _unitTestDependencyManuallyGenerator = unitTestDependencyManuallyGenerator;
        _compiler = compiler;
        _runner = runner;

        OutputBoxViewModel = outputBoxViewModelFactory.Create();

        ExecuteCommand = new AsyncRelayCommand(ExecuteAsync);
    }

    public Task OnNavigatedToAsync(NavigationContext context)
    {
        Input =
            @"
using System;
using System.Linq;
using System.IO;

Console.WriteLine(""Output"");

";

        return Task.CompletedTask;
    }

    private Task ExecuteAsync()
    {
        File.WriteAllText("DynamicProgram.cs", Input);

        var result = _runner.Execute(_compiler.Compile("DynamicProgram.cs"));

        OutputBoxViewModel?.SetOutput(result);

        return Task.CompletedTask;
    }

    public ICommand ExecuteCommand { get; set; }

    public string? Input
    {
        get => Get<string?>();
        set => Set(value);
    }

    public OutputBoxViewModel? OutputBoxViewModel
    {
        get => Get<OutputBoxViewModel?>();
        set => Set(value);
    }
}