using Jab;
using TypeCode.Business.Configuration.Assemblies;
using TypeCode.Business.Configuration.Location;

namespace TypeCode.Business.Configuration;

[ServiceProviderModule]
[Import(typeof(IConfigurationLocationModule))]
[Transient(typeof(IGenericXmlSerializer), typeof(GenericXmlSerializer))]
[Transient(typeof(IConfigurationMapper), typeof(ConfigurationMapper))]
[Singleton(typeof(IConfigurationProvider), typeof(ConfigurationProvider))]
[Transient(typeof(IConfigurationLoader), typeof(ConfigurationLoader))]
[Singleton(typeof(IAssemblyLoader), typeof(AssemblyLoader))]
[Singleton(typeof(IAssemblyDependencyLoader), typeof(AssemblyDependencyLoader))]
public interface IConfigurationModule
{
}