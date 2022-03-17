using Framework.Autofac.Boot;
using TypeCode.Business.Configuration;
using Workflow;

namespace TypeCode.Business.Bootstrapping;

internal class ConfigurationLoadBootStep<TContext> : IConfigurationLoadBootStep<TContext>
    where TContext : WorkflowBaseContext, IBootContext
{
    private readonly IConfigurationLoader _configurationLoader;
    private readonly IConfigurationProvider _configurationProvider;

    public ConfigurationLoadBootStep(IConfigurationLoader configurationLoader, IConfigurationProvider configurationProvider)
    {
        _configurationLoader = configurationLoader;
        _configurationProvider = configurationProvider;
    }

    public async Task ExecuteAsync(TContext context)
    {
        var configuration = await _configurationLoader.LoadAsync().ConfigureAwait(false);
        _configurationProvider.SetConfiguration(configuration);
    }

    public Task<bool> ShouldExecuteAsync(TContext context)
    {
        return context.ShouldExecuteAsync();
    }
}