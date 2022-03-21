using Framework.Jab.Boot;
using TypeCode.Business.Configuration;
using TypeCode.Business.TypeEvaluation;
using Workflow;

namespace TypeCode.Business.Bootstrapping;

public class ConfigurationLoadBootStep<TContext> : IConfigurationLoadBootStep<TContext>
    where TContext : WorkflowBaseContext, IBootContext
{
    private readonly IConfigurationLoader _configurationLoader;
    private readonly IConfigurationProvider _configurationProvider;
    private readonly ITypeProvider _typeProvider;

    public ConfigurationLoadBootStep(
        IConfigurationLoader configurationLoader,
        IConfigurationProvider configurationProvider,
        ITypeProvider typeProvider
        )
    {
        _configurationLoader = configurationLoader;
        _configurationProvider = configurationProvider;
        _typeProvider = typeProvider;
    }

    public async Task ExecuteAsync(TContext context)
    {
        var configuration = await _configurationLoader.LoadAsync().ConfigureAwait(false);
        _typeProvider.Initalize(configuration);
        _configurationProvider.SetConfiguration(configuration);
    }

    public Task<bool> ShouldExecuteAsync(TContext context)
    {
        return context.ShouldExecuteAsync();
    }
}