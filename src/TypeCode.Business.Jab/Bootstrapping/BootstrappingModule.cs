using Autofac;
using TypeCode.Business.Configuration;

namespace TypeCode.Business.Bootstrapping;

public class BootstrappingModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterGeneric(typeof(AssemblyLoadBootStep<>)).As(typeof(IAssemblyLoadBootStep<>));

        builder.RegisterModule<ConfigurationModule>();
            
        base.Load(builder);
    }
}