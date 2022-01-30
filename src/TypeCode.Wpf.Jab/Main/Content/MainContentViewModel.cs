using System.Threading.Tasks;
using TypeCode.Wpf.Jab.Application;
using TypeCode.Wpf.Jab.Helper.Event;
using TypeCode.Wpf.Jab.Helper.ViewModel;

namespace TypeCode.Wpf.Jab.Main.Content;

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