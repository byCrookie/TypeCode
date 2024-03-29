﻿using Jab;
using TypeCode.Business.Configuration.Assemblies;

namespace TypeCode.Business.Configuration;

[ServiceProviderModule]
[Transient(typeof(IGenericXmlSerializer), typeof(GenericXmlSerializer))]
[Transient(typeof(IConfigurationMapper), typeof(ConfigurationMapper))]
[Singleton(typeof(IConfigurationProvider), typeof(ConfigurationProvider))]
[Transient(typeof(IConfigurationLoader), typeof(ConfigurationLoader))]
[Singleton(typeof(IAssemblyLoader), typeof(AssemblyLoader))]
[Singleton(typeof(IAssemblyDependencyLoader), typeof(AssemblyDependencyLoader))]
public interface IConfigurationModule
{
}