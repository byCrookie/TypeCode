using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TypeCode.Wpf.Application;
using TypeCode.Wpf.Helper.Event;

namespace TypeCode.Wpf.Components.InputBox;

public partial class InputBoxViewModel : ObservableObject, IAsyncEventHandler<LoadStartEvent>, IAsyncEventHandler<LoadEndEvent>
{
    private readonly InputBoxViewModelParameter _parameter;
    private bool _loaded;

    public InputBoxViewModel(IEventAggregator eventAggregator, InputBoxViewModelParameter parameter)
    {
        _loaded = true;
        ActionName = parameter.ActionName;
        ToolTip = parameter.ToolTip;
        _parameter = parameter;

        eventAggregator.Subscribe<LoadStartEvent>(this);
        eventAggregator.Subscribe<LoadEndEvent>(this);
    }

    [RelayCommand(CanExecute = nameof(CanAction))]
    private Task ActionAsync()
    {
        return _parameter.ActionAsync(Regex, Input);
    }
    
    private bool CanAction()
    {
        return _loaded && !string.IsNullOrEmpty(Input?.Trim());
    }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ActionCommand))]
    private string? _input;

    [ObservableProperty]
    private string? _toolTip;

    [ObservableProperty]
    private string? _actionName;

    [ObservableProperty]
    private bool _regex;

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