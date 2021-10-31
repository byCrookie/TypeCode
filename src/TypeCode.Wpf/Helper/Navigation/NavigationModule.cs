using Autofac;
using TypeCode.Wpf.Helper.Navigation.Modal;
using TypeCode.Wpf.Helper.Navigation.Service;

namespace TypeCode.Wpf.Helper.Navigation
{
    public class NavigationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<NavigationService>().As<INavigationService>().SingleInstance();

            builder.RegisterModule<ModalModule>();
            
            base.Load(builder);
        }
    }
}