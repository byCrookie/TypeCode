using Jab;
using TypeCode.Business.Bootstrapping;
using TypeCode.Business.Configuration;
using TypeCode.Business.Mode;
using TypeCode.Business.TypeEvaluation;

namespace TypeCode.Business.Modules;

[ServiceProviderModule]
[Import(typeof(IConfigurationModule))]
[Import(typeof(IModeModule))]
[Import(typeof(ITypeEvaluationModule))]
[Singleton(typeof(IConfigurationProvider), typeof(ConfigurationProvider))]
public partial interface ITypeCodeBusinessModule
{
}