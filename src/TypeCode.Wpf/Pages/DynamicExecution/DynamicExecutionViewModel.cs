using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TypeCode.Business.Embedded;
using TypeCode.Business.Mode.DynamicExecution;
using TypeCode.Wpf.Components.OutputBox;
using TypeCode.Wpf.Helper.Navigation.Contract;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.Thread;
using TypeCode.Wpf.Helper.ViewModels;

namespace TypeCode.Wpf.Pages.DynamicExecution;

public sealed partial class DynamicExecutionViewModel : ViewModelBase, IAsyncInitialNavigated
{
    private readonly IOutputBoxViewModelFactory _outputBoxViewModelFactory;
    private readonly ICompiler _compiler;
    private readonly IRunner _runner;
    private readonly IResourceReader _resourceReader;

    public DynamicExecutionViewModel(
        IOutputBoxViewModelFactory outputBoxViewModelFactory,
        ICompiler compiler,
        IRunner runner,
        IResourceReader resourceReader
    )
    {
        _outputBoxViewModelFactory = outputBoxViewModelFactory;
        _compiler = compiler;
        _runner = runner;
        _resourceReader = resourceReader;
    }

    public Task OnInititalNavigationAsync(NavigationContext context)
    {
        OutputBoxViewModel = _outputBoxViewModelFactory.Create();
        Input = _resourceReader.ReadResource(Assembly.GetExecutingAssembly(), "Pages.DynamicExecution.DynamicExecutionTemplate.txt");
        return Task.CompletedTask;
    }

    [RelayCommand(CanExecute = nameof(CanExecute))]
    private Task ExecuteAsync()
    {
        MainThread.BackgroundFireAndForgetAsync(() =>
        {
            var result = _runner.Execute(_compiler.Compile(Input!));
            OutputBoxViewModel?.SetOutput(result);
        }, DispatcherPriority.Normal);
        return Task.CompletedTask;
    }

    private bool CanExecute()
    {
        return !HasErrors && !string.IsNullOrEmpty(Input);
    }

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [NotifyCanExecuteChangedFor(nameof(ExecuteCommand))]
    [Required]
    private string? _input;

    [ObservableProperty]
    [ChildViewModel]
    private OutputBoxViewModel? _outputBoxViewModel;
}