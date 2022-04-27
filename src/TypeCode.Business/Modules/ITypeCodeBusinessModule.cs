using Jab;
using TypeCode.Business.Bootstrapping;
using TypeCode.Business.Configuration;
using TypeCode.Business.Embedded;
using TypeCode.Business.Mode;
using TypeCode.Business.TypeEvaluation;
using TypeCode.Business.Version;

namespace TypeCode.Business.Modules;

[ServiceProviderModule]
[Import(typeof(IVersionModule))]
[Import(typeof(IConfigurationModule))]
[Import(typeof(IModeModule))]
[Import(typeof(ITypeEvaluationModule))]
[Import(typeof(IBootstrappingModule))]
[Import(typeof(IEmbeddedModule))]
[Singleton(typeof(IConfigurationProvider), typeof(ConfigurationProvider))]
public interface ITypeCodeBusinessModule
{
}