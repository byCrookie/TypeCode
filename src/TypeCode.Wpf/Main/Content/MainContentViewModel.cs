using TypeCode.Wpf.Application;
using TypeCode.Wpf.Helper.Event;
using TypeCode.Wpf.Helper.ViewModel;

namespace TypeCode.Wpf.Main.Content;

public class MainContentViewModel : Reactive, IAsyncEventHandler<AssemblyLoadedEvent>
{
    public MainContentViewModel(IEventAggregator eventAggregator)
    {
        AreAssembliesLoading = true;
            
        eventAggregator.Subscribe<AssemblyLoadedEvent>(this);
    }
        
    public bool AreAssembliesLoading {
        get => Get<bool>();
        set => Set(value);
    }
        
    public Task HandleAsync(AssemblyLoadedEvent e)
    {
        AreAssembliesLoading = false;
        return Task.CompletedTask;
    }
}