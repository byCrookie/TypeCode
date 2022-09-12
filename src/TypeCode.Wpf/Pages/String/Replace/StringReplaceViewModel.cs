using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TypeCode.Wpf.Components.OutputBox;
using TypeCode.Wpf.Helper.Navigation.Contract;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.ViewModels;

namespace TypeCode.Wpf.Pages.String.Replace;

public sealed partial class StringReplaceViewModel : ViewModelBase, IAsyncInitialNavigated
{
    public StringReplaceViewModel(IOutputBoxViewModelFactory outputBoxViewModelFactory)
    {
        OutputBoxViewModel = outputBoxViewModelFactory.Create();
    }
    
    public Task OnInititalNavigationAsync(NavigationContext context)
    {
        ReplaceItems = new ObservableCollection<ReplaceItemViewModel>();
        return Task.CompletedTask;
    }

    [RelayCommand]
    private Task RemoveEscapeAsync(ReplaceItemViewModel replaceItemViewModel)
    {
        ReplaceItems?.Remove(replaceItemViewModel);
        StringReplaceCommand.NotifyCanExecuteChanged();
        return Task.CompletedTask;
    }

    [RelayCommand(CanExecute = nameof(CanStringReplace))]
    private Task StringReplaceAsync()
    {
        var replacedText = ReplaceItems?.Aggregate(Input ?? string.Empty, (text, item) => text.Replace(item.ToReplace, item.ReplaceWith));
        OutputBoxViewModel?.SetOutput(replacedText);
        return Task.CompletedTask;
    }

    private bool CanStringReplace()
    {
        return !HasErrors && !string.IsNullOrEmpty(Input) && ReplaceItems is not null && ReplaceItems.Any();
    }
    
    [RelayCommand(CanExecute = nameof(CanAddReplace))]
    private Task AddReplaceAsync()
    {
        ReplaceItems?.Add(new ReplaceItemViewModel(ToReplace!, ReplaceWith!));
        StringReplaceCommand.NotifyCanExecuteChanged();
        return Task.CompletedTask;
    }

    private bool CanAddReplace()
    {
        return !string.IsNullOrEmpty(ToReplace) && !string.IsNullOrEmpty(ReplaceWith);
    }
    
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AddReplaceCommand))]
    private string? _toReplace;
    
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AddReplaceCommand))]
    private string? _replaceWith;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [NotifyCanExecuteChangedFor(nameof(StringReplaceCommand))]
    [Required]
    private string? _input;
    
    [ObservableProperty]
    private ObservableCollection<ReplaceItemViewModel>? _replaceItems;

    [ObservableProperty]
    [ChildViewModel]
    private OutputBoxViewModel? _outputBoxViewModel;
}