using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TypeCode.Wpf.Components.OutputBox;
using TypeCode.Wpf.Helper.ViewModels;

namespace TypeCode.Wpf.Pages.String.Length;

public partial class StringLengthViewModel : ViewModelBase
{
    public StringLengthViewModel(IOutputBoxViewModelFactory outputBoxViewModelFactory)
    {
        OutputBoxViewModel = outputBoxViewModelFactory.Create();
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