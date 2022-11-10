using Jab;

namespace TypeCode.Business.TypeEvaluation;

[ServiceProviderModule]
[Singleton(typeof(ILazyTypeProviderFactory), typeof(LazyTypeProviderFactory))]
[Transient(typeof(IConfigurationTypeProvider), typeof(ConfigurationTypeProvider))]
[Transient(typeof(IDllTypeProvider), typeof(DllTypeProvider))]
public interface ITypeEvaluationModule
{
}