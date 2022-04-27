using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Input;
using TypeCode.Business.Embedded;
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
    private readonly ICompiler _compiler;
    private readonly IRunner _runner;
    private readonly IResourceReader _resourceReader;

    public DynamicExecuteViewModel(
        IOutputBoxViewModelFactory outputBoxViewModelFactory,
        ICompiler compiler,
        IRunner runner,
        IResourceReader resourceReader
    )
    {
        _compiler = compiler;
        _runner = runner;
        _resourceReader = resourceReader;

        OutputBoxViewModel = outputBoxViewModelFactory.Create();

        ExecuteCommand = new AsyncRelayCommand(ExecuteAsync);
    }

    public Task OnNavigatedToAsync(NavigationContext context)
    {
        Input = _resourceReader.ReadResource(Assembly.GetExecutingAssembly(), "Pages.DynamicExecute.DynamicExecutionTemplate.txt");

        return Task.CompletedTask;
    }

    private Task ExecuteAsync()
    {
        var result = _runner.Execute(_compiler.Compile(Input!));

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