using Framework.DependencyInjection.Factory;
using TypeCode.Wpf.Helper.Event;
using TypeCode.Wpf.Helper.ViewModel;
using TypeCode.Wpf.Main.Content;
using TypeCode.Wpf.Main.Sidebar;

namespace TypeCode.Wpf.Main;

public class MainViewModel : Reactive, IAsyncEventHandler<VersionLoadedEvent>
{
    public MainViewModel(IFactory factory, IEventAggregator eventAggregator)
    {
        eventAggregator.Subscribe<VersionLoadedEvent>(this);
        
        Title = "TypeCode";
        
        MainContentViewModel = factory.Create<MainContentViewModel>();
        MainSidebarViewModel = factory.Create<MainSidebarViewModel>();
    }

    public MainContentViewModel MainContentViewModel { get; set; }
    public MainSidebarViewModel MainSidebarViewModel { get; set; }
    
    public string? Title
    {
        get => Get<string?>();
        set => Set(value);
    }

    public Task HandleAsync(VersionLoadedEvent e)
    {
        Title = $"TypeCode {e.Version}";
        return Task.CompletedTask;
    }
}