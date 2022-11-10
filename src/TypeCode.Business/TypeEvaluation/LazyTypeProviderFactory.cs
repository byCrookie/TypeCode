using TypeCode.Business.Configuration;

namespace TypeCode.Business.TypeEvaluation;

public class LazyTypeProviderFactory : ILazyTypeProviderFactory
{
    private readonly IConfigurationTypeProvider _configurationTypeProvider;
    private readonly IDllTypeProvider _dllTypeProvider;
    private Lazy<Task<ITypeProvider>>? _typeProviderLazy;

    public LazyTypeProviderFactory(
        IConfigurationTypeProvider configurationTypeProvider,
        IDllTypeProvider dllTypeProvider
        )
    {
        _configurationTypeProvider = configurationTypeProvider;
        _dllTypeProvider = dllTypeProvider;
    }

    public void InitializeByConfiguration(TypeCodeConfiguration typeCodeConfiguration)
    {
        _typeProviderLazy = new Lazy<Task<ITypeProvider>>(async () =>
        {
            await _configurationTypeProvider.InitalizeAsync(typeCodeConfiguration).ConfigureAwait(false);
            return _configurationTypeProvider;
        });
    }

    public void InitializeByDlls(IEnumerable<string> targetDllPaths)
    {
        _typeProviderLazy = new Lazy<Task<ITypeProvider>>(async () =>
        {
            await _dllTypeProvider.InitalizeAsync(targetDllPaths).ConfigureAwait(false);
            return _dllTypeProvider;
        });
    }

    public Task<ITypeProvider> ValueAsync()
    {
        if (_typeProviderLazy is null)
        {
            throw new NullReferenceException($"{nameof(LazyTypeProviderFactory)} needs to be initialized first.");
        }

        return _typeProviderLazy.Value;
    }
}