using TypeCode.Wpf.Main.Content;
using TypeCode.Wpf.Main.Sidebar;

namespace TypeCode.Wpf.Main;

public interface IMainViewProvider
{
    public MainWindow MainWindow();
    public MainContentView MainContentView();
    public MainSidebarView MainSidebarView();
}