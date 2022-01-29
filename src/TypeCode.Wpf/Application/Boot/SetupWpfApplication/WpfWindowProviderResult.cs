using TypeCode.Wpf.Main;
using TypeCode.Wpf.Main.Content;
using TypeCode.Wpf.Main.Sidebar;

namespace TypeCode.Wpf.Application.Boot.SetupWpfApplication;

public class WpfWindowProviderResult
{
    public MainWindow MainWindow { get; set; }
    public MainContentView MainContentView { get; set; }
    public MainSidebarView MainSidebarView { get; set; }
}