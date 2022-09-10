using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TypeCode.Wpf.Components.OutputBox;
using TypeCode.Wpf.Helper.Navigation.Contract;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.ViewModels;

namespace TypeCode.Wpf.Pages.String.Escape;

public partial class StringEscapeViewModel : ViewModelBase, IAsyncNavigatedTo
{
    public StringEscapeViewModel(IOutputBoxViewModelFactory outputBoxViewModelFactory)
    {
        OutputBoxViewModel = outputBoxViewModelFactory.Create();
    }
    
    public Task OnNavigatedToAsync(NavigationContext context)
    {
        EscapeItems = new ObservableCollection<EscapeItemViewModel>();
        return Task.CompletedTask;
    }
    
    [RelayCommand]
    private Task RemoveEscapeAsync(EscapeItemViewModel escapeItemViewModel)
    {
        EscapeItems?.Remove(escapeItemViewModel);
        return Task.CompletedTask;
    }

    [RelayCommand(CanExecute = nameof(CanStringEscape))]
    private Task StringEscapeAsync()
    {
        var escaped = EscapeItems?.Aggregate(Input ?? string.Empty, (text, item) => text.Replace(item.ToEscape, item.EscapeWith));
        OutputBoxViewModel?.SetOutput(escaped);
        return Task.CompletedTask;
    }

    private bool CanStringEscape()
    {
        return !HasErrors && !string.IsNullOrEmpty(Input) && EscapeItems is not null && EscapeItems.Any();
    }
    
    [RelayCommand(CanExecute = nameof(CanAddEscape))]
    private Task AddEscapeAsync()
    {
        EscapeItems?.Add(new EscapeItemViewModel(ToEscape!, EscapeWith!));
        return Task.CompletedTask;
    }

    private bool CanAddEscape()
    {
        return !string.IsNullOrEmpty(ToEscape) && !string.IsNullOrEmpty(EscapeWith);
    }
    
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AddEscapeCommand))]
    private string? _toEscape;
    
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AddEscapeCommand))]
    private string? _escapeWith;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [NotifyCanExecuteChangedFor(nameof(StringEscapeCommand))]
    [Required]
    private string? _input;
    
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [NotifyCanExecuteChangedFor(nameof(StringEscapeCommand))]
    [Required]
    private ObservableCollection<EscapeItemViewModel>? _escapeItems;

    [ObservableProperty]
    [ChildViewModel]
    private OutputBoxViewModel? _outputBoxViewModel;
}