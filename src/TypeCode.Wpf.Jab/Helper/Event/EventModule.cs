using Autofac;

namespace TypeCode.Wpf.Jab.Helper.Event;

public class EventModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();
            
        base.Load(builder);
    }
}