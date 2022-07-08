using DependencyInjection.Factory;
using TypeCode.Wpf.Helper.Event;
using TypeCode.Wpf.Helper.Navigation.Contract;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.ViewModel;
using TypeCode.Wpf.Main.Content;
using TypeCode.Wpf.Main.Sidebar;

namespace TypeCode.Wpf.Main;

public class MainViewModel : Reactive, IAsyncNavigatedTo, IAsyncEventHandler<VersionLoadedEvent>
{
    private readonly NavigationContext _navigationContext;

    public MainViewModel(IFactory factory, IEventAggregator eventAggregator)
    {
        eventAggregator.Subscribe<VersionLoadedEvent>(this);
        
        Title = "TypeCode";
        
        MainContentViewModel = factory.Create<MainContentViewModel>();
        MainSidebarViewModel = factory.Create<MainSidebarViewModel>();

        _navigationContext = new NavigationContext();
    }
    
    public Task OnNavigatedToAsync(NavigationContext context)
    {
        if (MainContentViewModel is IAsyncNavigatedTo navContent)
        {
            navContent.OnNavigatedToAsync(_navigationContext);
        }
        
        if (MainSidebarViewModel is IAsyncNavigatedTo navSideBar)
        {
            navSideBar.OnNavigatedToAsync(_navigationContext);
        }
        
        return Task.CompletedTask;
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
        Title = $"TypeCode {e.CurrentVersion}";
        return Task.CompletedTask;
    }
}