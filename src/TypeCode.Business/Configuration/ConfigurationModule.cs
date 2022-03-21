using Autofac;
using TypeCode.Business.Configuration.Assemblies;

namespace TypeCode.Business.Configuration;

internal class ConfigurationModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<GenericXmlSerializer>().As<IGenericXmlSerializer>();
        builder.RegisterType<ConfigurationMapper>().As<IConfigurationMapper>();
        builder.RegisterType<ConfigurationProvider>().As<IConfigurationProvider>().SingleInstance();
        builder.RegisterType<ConfigurationLoader>().As<IConfigurationLoader>();
        builder.RegisterType<AssemblyLoader>().As<IAssemblyLoader>();

        base.Load(builder);
    }
}