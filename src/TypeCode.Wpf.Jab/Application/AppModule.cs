using Autofac;
using Framework.Autofac.Boot;
using TypeCode.Wpf.Jab.Application.Boot;

namespace TypeCode.Wpf.Jab.Application;

public class AppModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<Application>().As<IApplication>();
            
        builder.RegisterModule<BootModule>();
            
        base.Load(builder);
    }
}