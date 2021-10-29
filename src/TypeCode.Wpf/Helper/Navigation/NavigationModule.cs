using Autofac;

namespace TypeCode.Wpf.Helper.Navigation
{
    public class NavigationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<NavigationService>().As<INavigationService>();
            
            base.Load(builder);
        }
    }
}