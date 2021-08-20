using Autofac;

namespace TypeCode.Business.Configuration
{
    internal class ConfigurationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<GenericXmlSerializer>().As<IGenericXmlSerializer>();
        }
    }
}