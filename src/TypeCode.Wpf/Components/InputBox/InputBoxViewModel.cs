using AsyncAwaitBestPractices.MVVM;
using TypeCode.Wpf.Application;
using TypeCode.Wpf.Helper.Commands;
using TypeCode.Wpf.Helper.Event;
using TypeCode.Wpf.Helper.ViewModel;

namespace TypeCode.Wpf.Components.InputBox;

public class InputBoxViewModel : Reactive, IAsyncEventHandler<LoadStartEvent>, IAsyncEventHandler<LoadEndEvent>
{
    private bool _loaded;

    public InputBoxViewModel(IEventAggregator eventAggregator, InputBoxViewModelParameter parameter)
    {
        _loaded = true;
        ActionName = parameter.ActionName;
        ToolTip = parameter.ToolTip;

        eventAggregator.Subscribe<LoadStartEvent>(this);
        eventAggregator.Subscribe<LoadEndEvent>(this);
        
        ActionCommand = new AsyncRelayCommand(() => parameter.ActionAsync(Regex, Input), _ => _loaded && !string.IsNullOrEmpty(Input?.Trim()));
    }
    
    public IAsyncCommand ActionCommand { get; set; }
    
    public string? Input
    {
        get => Get<string?>();
        set
        {
            Set(value);
            ActionCommand.RaiseCanExecuteChanged();
        }
    }

    public string? ToolTip
    {
        get => Get<string?>();
        set => Set(value);
    }
    
    public string? ActionName
    {
        get => Get<string?>();
        set => Set(value);
    }
    
    public bool Regex
    {
        get => Get<bool>();
        set => Set(value);
    }

    public Task HandleAsync(LoadStartEvent e)
    {
        _loaded = false;
        ActionCommand.RaiseCanExecuteChanged();
        return Task.CompletedTask;
    }
    
    public Task HandleAsync(LoadEndEvent e)
    {
        _loaded = true;
        ActionCommand.RaiseCanExecuteChanged();
        return Task.CompletedTask;
    }
}