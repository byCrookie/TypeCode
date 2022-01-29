using System;
using TypeCode.Wpf.Main.Content;
using TypeCode.Wpf.Main.Sidebar;

namespace TypeCode.Wpf.Main;

public class MainViewModel
{
    public MainViewModel(IServiceProvider serviceProvider)
    {
        MainContentViewModel = serviceProvider.GetService(typeof(MainContentViewModel)) as MainContentViewModel;
        MainSidebarViewModel = serviceProvider.GetService(typeof(MainSidebarViewModel)) as MainSidebarViewModel;
    }

    public MainContentViewModel MainContentViewModel { get; set; }
    public MainSidebarViewModel MainSidebarViewModel { get; set; }
}