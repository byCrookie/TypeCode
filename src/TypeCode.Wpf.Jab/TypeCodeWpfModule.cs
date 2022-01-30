using Autofac;
using TypeCode.Wpf.Jab.Application;
using TypeCode.Wpf.Jab.Helper;
using TypeCode.Wpf.Jab.Pages;

namespace TypeCode.Wpf.Jab;

public class TypeCodeWpfModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterModule<AppModule>();
        builder.RegisterModule<HelperModule>();
        builder.RegisterModule<PagesModule>();
            
        base.Load(builder);
    }
}