using Jab;
using TypeCode.Wpf.Main.Content;
using TypeCode.Wpf.Main.Sidebar;

namespace TypeCode.Wpf.Main;

[ServiceProviderModule]
[Singleton(typeof(IMainViewProvider), typeof(MainViewProvider))]
[Transient(typeof(MainViewModel))]
[Transient(typeof(MainContentViewModel))]
[Transient(typeof(MainSidebarViewModel))]
public interface IMainModule
{
    
}