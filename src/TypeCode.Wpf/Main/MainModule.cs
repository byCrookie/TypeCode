using Autofac;
using TypeCode.Wpf.Main.Content;
using TypeCode.Wpf.Main.Sidebar;

namespace TypeCode.Wpf.Main
{
    public class MainModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MainViewModel>().AsSelf();
            builder.RegisterType<MainContentViewModel>().AsSelf();
            builder.RegisterType<MainSidebarViewModel>().AsSelf();
            
            base.Load(builder);
        }
    }
}