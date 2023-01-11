using System.Diagnostics;
using System.IO;
using System.Reflection;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TypeCode.Business.Bootstrapping.Data;
using TypeCode.Business.Embedded;
using TypeCode.Business.Mode.DynamicExecution;
using TypeCode.Wpf.Components.OutputBox;
using TypeCode.Wpf.Helper.Navigation.Contract;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.ViewModels;
using TypeCode.Wpf.Helper.ViewModels.Flag;

namespace TypeCode.Wpf.Pages.DynamicExecution;

public sealed partial class DynamicExecutionViewModel : ViewModelBase, IAsyncInitialNavigated
{
    private readonly IOutputBoxViewModelFactory _outputBoxViewModelFactory;
    private readonly ICompiler _compiler;
    private readonly IRunner _runner;
    private readonly IResourceReader _resourceReader;
    private readonly IUserDataLocationProvider _userDataLocationProvider;
    private readonly IFlagScopeFactory _flagScopeFactory;

    private readonly Flag _isCodeOpen;
    private readonly Flag _isExecuting;

    public DynamicExecutionViewModel(
        IOutputBoxViewModelFactory outputBoxViewModelFactory,
        ICompiler compiler,
        IRunner runner,
        IResourceReader resourceReader,
        IUserDataLocationProvider userDataLocationProvider,
        IFlagScopeFactory flagScopeFactory
    )
    {
        _outputBoxViewModelFactory = outputBoxViewModelFactory;
        _compiler = compiler;
        _runner = runner;
        _resourceReader = resourceReader;
        _userDataLocationProvider = userDataLocationProvider;
        _flagScopeFactory = flagScopeFactory;

        _isCodeOpen = new Flag();
        _isExecuting = new Flag();
    }

    public async Task OnInititalNavigationAsync(NavigationContext context)
    {
        OutputBoxViewModel = _outputBoxViewModelFactory.Create();

        if (!File.Exists(_userDataLocationProvider.GetDynamicExecutionPath()) || await DynamicExecutionCodeIsEmptyAsync())
        {
            var template = _resourceReader.ReadResource(Assembly.GetExecutingAssembly(), "Pages.DynamicExecution.DynamicExecutionTemplate.txt");
            await File.WriteAllTextAsync(_userDataLocationProvider.GetDynamicExecutionPath(), template);
        }
    }

    private async Task<bool> DynamicExecutionCodeIsEmptyAsync()
    {
        var content = await File.ReadAllTextAsync(_userDataLocationProvider.GetDynamicExecutionPath());
        return string.IsNullOrEmpty(content);
    }

    [RelayCommand(CanExecute = nameof(CanExecuteOpenCode))]
    private async Task OpenCodeAsync()
    {
        await using (_flagScopeFactory.CreateAsync(_isCodeOpen, _ => NotifyCommandsAsync()))
        {
            var processInfo = new ProcessStartInfo
            {
                FileName = _userDataLocationProvider.GetDynamicExecutionPath(),
                UseShellExecute = true
            };

            var process = Process.Start(processInfo);

            if (process is not null)
            {
                await process.WaitForExitAsync();
            }
        }
    }
    
    private bool CanExecuteOpenCode()
    {
        return !_isExecuting && !_isCodeOpen && File.Exists(_userDataLocationProvider.GetDynamicExecutionPath());
    }

    [RelayCommand(CanExecute = nameof(CanExecute))]
    private async Task ExecuteAsync()
    {
        await using (_flagScopeFactory.CreateAsync(_isExecuting, _ => NotifyCommandsAsync()))
        {
            var code = await File.ReadAllTextAsync(_userDataLocationProvider.GetDynamicExecutionPath());
            var result = await Task.Run(() => _runner.Execute(_compiler.Compile(code)));
            OutputBoxViewModel?.SetOutput(result);
        }
    }

    private bool CanExecute()
    {
        return !HasErrors && !_isExecuting && !_isCodeOpen && File.Exists(_userDataLocationProvider.GetDynamicExecutionPath());
    }

    [ObservableProperty]
    [ChildViewModel]
    private OutputBoxViewModel? _outputBoxViewModel;

    private Task NotifyCommandsAsync()
    {
        ExecuteCommand.NotifyCanExecuteChanged();
        OpenCodeCommand.NotifyCanExecuteChanged();
        return Task.CompletedTask;
    }
}