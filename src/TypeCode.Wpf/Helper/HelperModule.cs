using Autofac;
using TypeCode.Wpf.Helper.Event;
using TypeCode.Wpf.Helper.Modal;
using TypeCode.Wpf.Helper.Navigation;

namespace TypeCode.Wpf.Helper
{
    public class HelperModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<NavigationModule>();
            builder.RegisterModule<EventModule>();
            builder.RegisterModule<ModalModule>();
            
            base.Load(builder);
        }
    }
}