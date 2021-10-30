using Autofac;

namespace TypeCode.Wpf.Specflow
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