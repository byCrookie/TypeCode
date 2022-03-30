using TypeCode.Wpf.Helper.Event;
using TypeCode.Wpf.Helper.ViewModel;
using TypeCode.Wpf.Main.Content;

namespace TypeCode.Wpf.Pages.Home;

public class HomeViewModel : Reactive, IAsyncEventHandler<VersionLoadedEvent>
{
    public HomeViewModel(IEventAggregator eventAggregator)
    {
        eventAggregator.Subscribe<VersionLoadedEvent>(this);
    }

    public bool Recursive
    {
        get => Get<bool>();
        set => Set(value);
    }

    public Task HandleAsync(VersionLoadedEvent e)
    {
       return Task.CompletedTask;
    }
}