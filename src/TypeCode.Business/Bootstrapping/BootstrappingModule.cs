using Autofac;

namespace TypeCode.Business.Bootstrapping
{
    public class BootstrappingModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(AssemblyLoadBootStep<>)).As(typeof(IAssemblyLoadBootStep<>));
            
            base.Load(builder);
        }
    }
}