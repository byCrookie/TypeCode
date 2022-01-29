using Jab;

namespace TypeCode.Business.TypeEvaluation;

[ServiceProviderModule]
[Singleton(typeof(ITypeProvider), typeof(TypeProvider))]
[Transient(typeof(ITypeEvaluator), typeof(TypeEvaluator))]
internal partial interface ITypeEvaluationModule
{
}