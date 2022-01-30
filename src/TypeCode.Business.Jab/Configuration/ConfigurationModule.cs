using Autofac;

namespace TypeCode.Business.Configuration;

internal class ConfigurationModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<GenericXmlSerializer>().As<IGenericXmlSerializer>();
        builder.RegisterType<ConfigurationMapper>().As<IConfigurationMapper>();
            
        base.Load(builder);
    }
}