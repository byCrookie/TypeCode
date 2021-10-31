using Autofac;
using TypeCode.Wpf.Application;
using TypeCode.Wpf.Helper;
using TypeCode.Wpf.Main;
using TypeCode.Wpf.Pages;

namespace TypeCode.Wpf
{
    public class TypeCodeWpfModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<AppModule>();
            builder.RegisterModule<HelperModule>();
            builder.RegisterModule<MainModule>();
            builder.RegisterModule<PagesModule>();
            
            base.Load(builder);
        }
    }
}