using Jab;

namespace TypeCode.Business.TypeEvaluation;

[ServiceProviderModule]
[Transient(typeof(ITypeEvaluator), typeof(TypeEvaluator))]
[Singleton(typeof(ITypeProvider), typeof(TypeProvider))]
public interface ITypeEvaluationModule
{
}