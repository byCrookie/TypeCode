using Autofac;
using TypeCode.Wpf.Helper.Autofac;
using TypeCode.Wpf.Main.Content;
using TypeCode.Wpf.Main.Sidebar;

namespace TypeCode.Wpf.Main
{
    public class MainModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // builder.AddViewModelAndViewAsSingleInstance<MainViewModel, MainWindow>();
            // builder.AddViewModelAndViewAsSingleInstance<MainContentViewModel, MainContentView>();
            // builder.AddViewModelAndViewAsSingleInstance<MainSidebarViewModel, MainSidebarView>();

            base.Load(builder);
        }
    }
}