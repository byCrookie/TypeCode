using Autofac;
using TypeCode.Wpf.Helper.Navigation;

namespace TypeCode.Wpf.Helper
{
    public class HelperModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<NavigationModule>();
            
            base.Load(builder);
        }
    }
}