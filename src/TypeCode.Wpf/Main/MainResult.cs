using TypeCode.Wpf.Main.Content;
using TypeCode.Wpf.Main.Sidebar;

namespace TypeCode.Wpf.Main;

public class MainResult
{
    public MainResult(MainWindow mainWindow, MainContentView mainContent, MainSidebarView mainSidebar)
    {
        MainWindow = mainWindow;
        MainContent = mainContent;
        MainSidebar = mainSidebar;
    }

    public MainWindow MainWindow { get; }
    public MainContentView MainContent { get; }
    public MainSidebarView MainSidebar { get; }
}