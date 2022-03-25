using Jab;
using TypeCode.Wpf.Main.Content;
using TypeCode.Wpf.Main.Sidebar;

namespace TypeCode.Wpf.Main;

[ServiceProviderModule]
[Singleton(typeof(IMainViewProvider), typeof(MainViewProvider))]
[Singleton(typeof(MainViewModel))]
[Singleton(typeof(MainContentViewModel))]
[Singleton(typeof(MainSidebarViewModel))]
public interface IMainModule
{
    
}