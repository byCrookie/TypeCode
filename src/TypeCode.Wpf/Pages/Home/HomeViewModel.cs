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

    public string? Version
    {
        get => Get<string?>();
        set => Set(value);
    }

    public Task HandleAsync(VersionLoadedEvent e)
    {
        Version = e.Version;
        return Task.CompletedTask;
    }
}