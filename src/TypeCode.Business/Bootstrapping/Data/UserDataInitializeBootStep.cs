using System.Reflection;
using Framework.Boot;
using Framework.Extensions.List;
using TypeCode.Business.Embedded;
using TypeCode.Business.Logging;
using Workflow;

namespace TypeCode.Business.Bootstrapping.Data;

public class UserDataInitializeBootStep<TContext> : IUserDataInitializeBootStep<TContext>
    where TContext : WorkflowBaseContext, IBootContext
{
    // ReSharper disable once UnusedMember.Local
    private const string ConfigurationProdFileName = "Configuration.Production.cfg.xml";

    // ReSharper disable once UnusedMember.Local
    private const string ConfigurationDevFileName = "Configuration.Development.cfg.xml";
    private const string ConfigurationFileName = "Configuration.cfg.xml";

    private readonly IResourceReader _resourceReader;
    private readonly IUserDataLocationInitializer _userDataLocationInitializer;

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
        var exeLocationBasePath = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\";
        var exeLogsLocation = $"{exeLocationBasePath}logs";
        var exeCacheLocation = $"{exeLocationBasePath}cache";
        var exeConfigurationLocation = $"{exeLocationBasePath}{ConfigurationFileName}";

        LogFileNames.AllNames.Select(name => Path.Combine(exeLogsLocation, name)).ForEach(path =>
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        });

        if (File.Exists(exeConfigurationLocation))
        {
            Directory.CreateDirectory(exeLogsLocation);
            Directory.CreateDirectory(exeCacheLocation);
            _userDataLocationInitializer.InitializeCachePath(exeCacheLocation);
            _userDataLocationInitializer.InitializeLogsPath(exeLogsLocation);
            _userDataLocationInitializer.InitializeConfigurationPath(exeConfigurationLocation);
            return;
        }

        var appDataLocationBasePath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\TypeCode\";
        var appDataLogsLocation = $@"{appDataLocationBasePath}logs";
        var appDataCacheLocation = $@"{appDataLocationBasePath}cache";
        var appDataConfigurationLocation = $@"{appDataLocationBasePath}{ConfigurationFileName}";

        LogFileNames.AllNames.Select(name => Path.Combine(appDataLogsLocation, name)).ForEach(path =>
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        });

        if (File.Exists(appDataConfigurationLocation))
        {
            Directory.CreateDirectory(appDataLogsLocation);
            Directory.CreateDirectory(appDataCacheLocation);
            _userDataLocationInitializer.InitializeCachePath(appDataCacheLocation);
            _userDataLocationInitializer.InitializeLogsPath(appDataLogsLocation);
            _userDataLocationInitializer.InitializeConfigurationPath(appDataConfigurationLocation);
            return;
        }

#if DEBUG
        var configTemplate = _resourceReader.ReadResource(Assembly.GetExecutingAssembly(), $"Bootstrapping.Data.{ConfigurationDevFileName}");
#else
        var configTemplate = _resourceReader.ReadResource(Assembly.GetExecutingAssembly(), $"Bootstrapping.Data.{ConfigurationProdFileName}");
#endif

        Directory.CreateDirectory(appDataCacheLocation);
        _userDataLocationInitializer.InitializeLogsPath(appDataCacheLocation);

        Directory.CreateDirectory(appDataLogsLocation);
        _userDataLocationInitializer.InitializeLogsPath(appDataLogsLocation);

        Directory.CreateDirectory(Path.GetDirectoryName(appDataConfigurationLocation) ?? throw new ArgumentException($"{appDataConfigurationLocation} is not valid location"));
        await File.WriteAllTextAsync(appDataConfigurationLocation, configTemplate).ConfigureAwait(false);
        _userDataLocationInitializer.InitializeConfigurationPath(appDataConfigurationLocation);
    }

    public Task<bool> ShouldExecuteAsync(TContext context)
    {
        return context.ShouldExecuteAsync();
    }
}