using TypeCode.Business.Configuration;

namespace TypeCode.Business.TypeEvaluation;

public interface ILazyTypeProviderFactory
{
    void InitializeByConfiguration(TypeCodeConfiguration typeCodeConfiguration);
    void InitializeByDlls(IEnumerable<string> targetDllPaths);
    Task<ITypeProvider> ValueAsync();
}