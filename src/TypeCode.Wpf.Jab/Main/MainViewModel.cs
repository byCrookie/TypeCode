using Framework.DependencyInjection.Factory;
using TypeCode.Wpf.Jab.Main.Content;
using TypeCode.Wpf.Jab.Main.Sidebar;

namespace TypeCode.Wpf.Jab.Main;

public class MainViewModel
{
    public MainViewModel(
        IFactory<MainContentViewModel> contentViewModelFactory,
        IFactory<MainSidebarViewModel> sidebarViewModelFactory
    )
    {
        MainContentViewModel = contentViewModelFactory.Create();
        MainSidebarViewModel = sidebarViewModelFactory.Create();
    }

    public MainContentViewModel MainContentViewModel { get; set; }
    public MainSidebarViewModel MainSidebarViewModel { get; set; }
}