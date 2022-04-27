using System.CodeDom.Compiler;
using System.Windows.Input;
using Microsoft.CSharp;
using TypeCode.Business.Mode;
using TypeCode.Business.Mode.UnitTestDependency.Manually;
using TypeCode.Wpf.Components.OutputBox;
using TypeCode.Wpf.Helper.Commands;
using TypeCode.Wpf.Helper.Navigation.Contract;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.ViewModel;

namespace TypeCode.Wpf.Pages.DynamicExecute;

public class DynamicExecuteViewModel : Reactive, IAsyncNavigatedTo
{
    private readonly ITypeCodeGenerator<UnitTestDependencyManuallyGeneratorParameter> _unitTestDependencyManuallyGenerator;

    public DynamicExecuteViewModel(
        ITypeCodeGenerator<UnitTestDependencyManuallyGeneratorParameter> unitTestDependencyManuallyGenerator,
        IOutputBoxViewModelFactory outputBoxViewModelFactory
    )
    {
        _unitTestDependencyManuallyGenerator = unitTestDependencyManuallyGenerator;

        OutputBoxViewModel = outputBoxViewModelFactory.Create();

        ExecuteCommand = new AsyncRelayCommand(ExecuteAsync);
    }

    public Task OnNavigatedToAsync(NavigationContext context)
    {
        Input =
            @"using System.Linq;

class Program 
{
    public static void Main(string[] args) 
    {
                
    }
}";

        return Task.CompletedTask;
    }

    private Task ExecuteAsync()
    {
        var csc = new CSharpCodeProvider(new Dictionary<string, string>() { { "CompilerVersion", "v3.5" } });
        var parameters = new CompilerParameters(new[] { "mscorlib.dll", "System.Core.dll" }, "foo.exe", true)
        {
            GenerateExecutable = true
        };

        var results = csc.CompileAssemblyFromSource(parameters, Input);

        OutputBoxViewModel?.SetOutput(results.Output.ToString());
        results.Errors.Cast<CompilerError>()
            .ToList()
            .ForEach(error => OutputBoxViewModel?.SetOutput($"{OutputBoxViewModel.Output}{Environment.NewLine}{error.ErrorText}"));

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