using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TypeCode.Wpf.Application;
using TypeCode.Wpf.Helper.Event;
using TypeCode.Wpf.Helper.Navigation.Contract;
using TypeCode.Wpf.Helper.Navigation.Service;

namespace TypeCode.Wpf.Components.InputBox;

public partial class InputBoxViewModel :
    ObservableValidator,
    IAsyncEventHandler<LoadStartEvent>,
    IAsyncEventHandler<LoadEndEvent>,
    IAsyncNavigatedFrom
{
    private readonly IEventAggregator _eventAggregator;
    private readonly InputBoxViewModelParameter _parameter;
    private bool _loaded;

    public InputBoxViewModel(IEventAggregator eventAggregator, InputBoxViewModelParameter parameter)
    {
        _eventAggregator = eventAggregator;

        _loaded = true;
        ActionName = parameter.ActionName;
        ToolTip = parameter.ToolTip;
        _parameter = parameter;

        eventAggregator.Subscribe<LoadStartEvent>(this);
        eventAggregator.Subscribe<LoadEndEvent>(this);
    }

    public Task OnNavigatedFromAsync(NavigationContext context)
    {
        _eventAggregator.Unsubscribe(this);
        return Task.CompletedTask;
    }

    [RelayCommand(CanExecute = nameof(CanAction))]
    private Task ActionAsync()
    {
        return _parameter.ActionAsync(UseRegexSearch, Input);
    }

    private bool CanAction()
    {
        return _loaded && !HasErrors && !string.IsNullOrEmpty(Input);
    }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ActionCommand))]
    [NotifyDataErrorInfo]
    [Required]
    private string? _input;

    [ObservableProperty]
    private string? _toolTip;

    [ObservableProperty]
    private string? _actionName;

    [ObservableProperty]
    private bool _useRegexSearch;

    public Task HandleAsync(LoadStartEvent e)
    {
        _loaded = false;
        ActionCommand.NotifyCanExecuteChanged();
        return Task.CompletedTask;
    }

    public Task HandleAsync(LoadEndEvent e)
    {
        _loaded = true;
        ActionCommand.NotifyCanExecuteChanged();
        return Task.CompletedTask;
    }
}