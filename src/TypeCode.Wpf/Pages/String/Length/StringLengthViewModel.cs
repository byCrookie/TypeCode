using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TypeCode.Wpf.Components.OutputBox;
using TypeCode.Wpf.Helper.Navigation.Contract;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.ViewModels;

namespace TypeCode.Wpf.Pages.String.Length;

public sealed partial class StringLengthViewModel : ViewModelBase, IAsyncInitialNavigated
{
    private readonly IOutputBoxViewModelFactory _outputBoxViewModelFactory;

    public StringLengthViewModel(IOutputBoxViewModelFactory outputBoxViewModelFactory)
    {
        _outputBoxViewModelFactory = outputBoxViewModelFactory;
    }
    
    public Task OnInititalNavigationAsync(NavigationContext context)
    {
        OutputBoxViewModel = _outputBoxViewModelFactory.Create();
        return Task.CompletedTask;
    }

    [RelayCommand(CanExecute = nameof(CanStringLength))]
    private Task StringLengthAsync()
    {
        OutputBoxViewModel?.SetOutput($"Length: {Input?.Length}{Environment.NewLine}Input has a length of {Input?.Length} characters.");
        return Task.CompletedTask;
    }
    
    private bool CanStringLength()
    {
        return !HasErrors && !string.IsNullOrEmpty(Input);
    }

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [NotifyCanExecuteChangedFor(nameof(StringLengthCommand))]
    [Required]
    private string? _input;

    [ObservableProperty]
    [ChildViewModel]
    private OutputBoxViewModel? _outputBoxViewModel;
}