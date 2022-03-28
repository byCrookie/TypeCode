using TypeCode.Business.Configuration;

namespace TypeCode.Business.TypeEvaluation;

public interface ITypeProvider
{
    Task InitalizeAsync(TypeCodeConfiguration configuration);
    bool HasByName(string? name);
    IEnumerable<Type> TryGetByName(string? name);
    IEnumerable<Type> TryGetByNames(IReadOnlyList<string> names);
    IEnumerable<Type> TryGetTypesByCondition(Func<Type, bool> condition);
}