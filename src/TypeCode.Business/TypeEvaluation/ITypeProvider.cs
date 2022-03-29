using TypeCode.Business.Configuration;

namespace TypeCode.Business.TypeEvaluation;

public interface ITypeProvider
{
    Task InitalizeAsync(TypeCodeConfiguration configuration);
    bool HasByName(string? name, TypeEvaluationOptions? options = null);
    IEnumerable<Type> TryGetByName(string? name, TypeEvaluationOptions? options = null);
    IEnumerable<Type> TryGetByNames(IReadOnlyList<string> names, TypeEvaluationOptions? options = null);
    IEnumerable<Type> TryGetTypesByCondition(Func<Type, bool> condition);
}