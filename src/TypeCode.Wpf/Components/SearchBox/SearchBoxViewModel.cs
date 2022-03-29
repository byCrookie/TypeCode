using AsyncAwaitBestPractices.MVVM;
using TypeCode.Wpf.Application;
using TypeCode.Wpf.Helper.Commands;
using TypeCode.Wpf.Helper.Event;
using TypeCode.Wpf.Helper.ViewModel;

namespace TypeCode.Wpf.Components.SearchBox;

public class SearchBoxViewModel : Reactive, IAsyncEventHandler<LoadStartEvent>, IAsyncEventHandler<LoadEndEvent>
{
    private bool _loaded;

    public SearchBoxViewModel(IEventAggregator eventAggregator, Func<bool, string?, Task> searchAsync)
    {
        _loaded = true;
        
        eventAggregator.Subscribe<LoadStartEvent>(this);
        eventAggregator.Subscribe<LoadEndEvent>(this);
        
        SearchCommand = new AsyncRelayCommand(() => searchAsync(Regex, Input), _ => _loaded);
    }
    
    public IAsyncCommand SearchCommand { get; set; }
    
    public string? Input
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
        SearchCommand.RaiseCanExecuteChanged();
        return Task.CompletedTask;
    }
    
    public Task HandleAsync(LoadEndEvent e)
    {
        _loaded = true;
        SearchCommand.RaiseCanExecuteChanged();
        return Task.CompletedTask;
    }
}