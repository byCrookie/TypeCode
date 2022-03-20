using Jab;

namespace TypeCode.Business.TypeEvaluation;

[ServiceProviderModule]
[Singleton(typeof(ITypeProvider), typeof(TypeProvider))]
public interface ITypeEvaluationModule
{
}