using Jab;

namespace TypeCode.Business.Configuration;

[ServiceProviderModule]
[Transient(typeof(IGenericXmlSerializer), typeof(GenericXmlSerializer))]
[Transient(typeof(IConfigurationMapper), typeof(ConfigurationMapper))]
[Singleton(typeof(IConfigurationProvider), typeof(ConfigurationProvider))]
[Transient(typeof(IConfigurationLoader), typeof(ConfigurationLoader))]
[Transient(typeof(IAssemblyFileLoader), typeof(AssemblyFileLoader))]
public interface IConfigurationModule
{
}