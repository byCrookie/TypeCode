using Autofac;
using TypeCode.Wpf.Jab.Helper.Event;
using TypeCode.Wpf.Jab.Helper.Navigation;

namespace TypeCode.Wpf.Jab.Helper;

public class HelperModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterModule<NavigationModule>();
        builder.RegisterModule<EventModule>();

        base.Load(builder);
    }
}