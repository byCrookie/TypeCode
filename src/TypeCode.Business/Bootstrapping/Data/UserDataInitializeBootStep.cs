using System.Reflection;
using Framework.Boot;
using TypeCode.Business.Embedded;
using TypeCode.Business.Logging;
using Workflow;

namespace TypeCode.Business.Bootstrapping.Data;

public sealed class UserDataInitializeBootStep<TContext> : IUserDataInitializeBootStep<TContext>
    where TContext : WorkflowBaseContext, IBootContext
{
    // ReSharper disable once UnusedMember.Local
    private const string ConfigurationProdFileName = "Configuration.Production.cfg.xml";

    // ReSharper disable once UnusedMember.Local
    private const string ConfigurationDevFileName = "Configuration.Development.cfg.xml";
    private const string ConfigurationFileName = "Configuration.cfg.xml";
    private const string DynamicExecutionFileName = "DynamicExecution.cs";

    private readonly IResourceReader _resourceReader;
    private readonly IUserDataLocationInitializer _userDataLocationInitializer;

    private const string SubLocationLogsName = "logs";
    private const string SubLocationCacheName = "cache";

    private readonly List<string> _priorityDataBaseLocations = new()
    {
        $@"{Path.GetDirectoryName(AppContext.BaseDirectory)}\",
        $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\TypeCode\"
    };

    private readonly List<string> _subLocations = new()
    {
        SubLocationLogsName,
        SubLocationCacheName
    };

    public UserDataInitializeBootStep(
        IResourceReader resourceReader,
        IUserDataLocationProvider userDataLocationInitializer
    )
    {
        _resourceReader = resourceReader;
        _userDataLocationInitializer = (IUserDataLocationInitializer)userDataLocationInitializer;
    }

    public async Task ExecuteAsync(TContext context)
    {
        var basePath = GetBasePathByPriority();
        Directory.CreateDirectory(basePath);

        foreach (var subLocation in _subLocations.Select(location => Path.Combine(basePath, location)))
        {
            Directory.CreateDirectory(subLocation);
        }

        foreach (var logfile in LogFileNames.AllNames.Select(name => Path.Combine(basePath, SubLocationLogsName, name)))
        {
            if (File.Exists(logfile))
            {
                File.Delete(logfile);
            }
        }

        _userDataLocationInitializer.InitializeCachePath(Path.Combine(basePath, SubLocationCacheName));
        _userDataLocationInitializer.InitializeLogsPath(Path.Combine(basePath, SubLocationLogsName));
        _userDataLocationInitializer.InitializeConfigurationFilePath(Path.Combine(basePath, ConfigurationFileName));
        _userDataLocationInitializer.InitializeDynamicExecutionPath(Path.Combine(basePath, DynamicExecutionFileName));

        if (File.Exists(Path.Combine(basePath, ConfigurationFileName)))
        {
            return;
        }

#if DEBUG
        var configTemplate = _resourceReader.ReadResource(Assembly.GetExecutingAssembly(), $"Bootstrapping.Data.{ConfigurationDevFileName}");
#else
        var configTemplate = _resourceReader.ReadResource(Assembly.GetExecutingAssembly(), $"Bootstrapping.Data.{ConfigurationProdFileName}");
#endif

        await File.WriteAllTextAsync(Path.Combine(basePath, ConfigurationFileName), configTemplate).ConfigureAwait(false);
    }

    private string GetBasePathByPriority()
    {
        var baseLocation = _priorityDataBaseLocations
            .FirstOrDefault(baseLocation => File.Exists(Path.Combine(baseLocation, ConfigurationFileName)));
        return baseLocation ?? _priorityDataBaseLocations.Last();
    }

    public Task<bool> ShouldExecuteAsync(TContext context)
    {
        return context.ShouldExecuteAsync();
    }
}