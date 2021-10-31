using Autofac;
using TypeCode.Business.Modules;

namespace TypeCode.Wpf.Application.Boot
{
    public class BootModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(SetupWpfApplication.SetupWpfApplicationStep<>)).As(typeof(SetupWpfApplication.ISetupWpfApplicationStep<>));

            builder.RegisterModule<TypeCodeBusinessModule>();
            
            base.Load(builder);
        }
    }
}