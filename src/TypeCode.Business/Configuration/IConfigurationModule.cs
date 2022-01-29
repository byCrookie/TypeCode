using Jab;

namespace TypeCode.Business.Configuration;

[ServiceProviderModule]
[Transient(typeof(IGenericXmlSerializer), typeof(GenericXmlSerializer))]
[Transient(typeof(IConfigurationMapper), typeof(ConfigurationMapper))]
internal partial interface IConfigurationModule
{
}