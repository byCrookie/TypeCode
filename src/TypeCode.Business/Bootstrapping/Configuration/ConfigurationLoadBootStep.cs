using Framework.Boot;
using TypeCode.Business.Configuration;
using TypeCode.Business.TypeEvaluation;
using Workflow;

namespace TypeCode.Business.Bootstrapping.Configuration;

public sealed class ConfigurationLoadBootStep<TContext> : IConfigurationLoadBootStep<TContext>
    where TContext : WorkflowBaseContext, IBootContext
{
    private readonly IConfigurationLoader _configurationLoader;
    private readonly IConfigurationProvider _configurationProvider;
    private readonly ILazyTypeProviderFactory _lazyTypeProviderFactory;

    public ConfigurationLoadBootStep(
        IConfigurationLoader configurationLoader,
        IConfigurationProvider configurationProvider,
        ILazyTypeProviderFactory lazyTypeProviderFactory
    )
    {
        _configurationLoader = configurationLoader;
        _configurationProvider = configurationProvider;
        _lazyTypeProviderFactory = lazyTypeProviderFactory;
    }

    public async Task ExecuteAsync(TContext context)
    {
        var configuration = await _configurationLoader.LoadAsync().ConfigureAwait(false);
        _lazyTypeProviderFactory.InitializeByConfiguration(configuration);
        await _lazyTypeProviderFactory.ValueAsync().ConfigureAwait(false);
        _configurationProvider.Set(configuration);
    }

    public Task<bool> ShouldExecuteAsync(TContext context)
    {
        return context.ShouldExecuteAsync();
    }
}