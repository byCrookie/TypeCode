using Autofac;

namespace TypeCode.Wpf.Pages.Specflow
{
    public class SpecflowModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SpecflowViewModel>().AsSelf();
            
            base.Load(builder);
        }
    }
}