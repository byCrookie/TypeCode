using Autofac;
using TypeCode.Business.Configuration;

namespace TypeCode.Business.Bootstrapping;

public class BootstrappingModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterGeneric(typeof(ConfigurationAutofacLoadBootStep<>)).As(typeof(IConfigurationAutofacLoadBootStep<>));

        builder.RegisterModule<ConfigurationModule>();
            
        base.Load(builder);
    }
}