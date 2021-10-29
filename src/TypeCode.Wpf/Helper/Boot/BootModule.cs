using Autofac;
using TypeCode.Business.Modules;
using TypeCode.Wpf.Helper.Boot.SetupWpfApplication;

namespace TypeCode.Wpf.Helper.Boot
{
    public class BootModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(SetupWpfApplicationStep<>)).As(typeof(ISetupWpfApplicationStep<>));

            builder.RegisterModule<TypeCodeBusinessModule>();
            
            base.Load(builder);
        }
    }
}