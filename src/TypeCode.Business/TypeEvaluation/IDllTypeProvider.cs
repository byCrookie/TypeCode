namespace TypeCode.Business.TypeEvaluation;

public interface IDllTypeProvider : ITypeProvider
{
    Task InitalizeAsync(IReadOnlyList<string> dllPaths, bool dllDeep, string dllPattern);
}