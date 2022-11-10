namespace TypeCode.Business.TypeEvaluation;

public interface IDllTypeProvider : ITypeProvider
{
    Task InitalizeAsync(IEnumerable<string> targetDllPaths);
}