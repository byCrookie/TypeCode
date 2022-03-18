using Framework.DependencyInjection.Factory;
using TypeCode.Wpf.Main.Content;
using TypeCode.Wpf.Main.Sidebar;

namespace TypeCode.Wpf.Main;

public class MainViewModel
{
    public MainViewModel(IFactory factory)
    {
        MainContentViewModel = factory.Create<MainContentViewModel>();
        MainSidebarViewModel = factory.Create<MainSidebarViewModel>();
    }

    public MainContentViewModel MainContentViewModel { get; set; }
    public MainSidebarViewModel MainSidebarViewModel { get; set; }
}