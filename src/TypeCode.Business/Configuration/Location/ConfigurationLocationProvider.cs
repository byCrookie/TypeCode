using System.Reflection;
using TypeCode.Business.Embedded;

namespace TypeCode.Business.Configuration.Location;

public class ConfigurationLocationProvider : IConfigurationLocationProvider
{
    private readonly IResourceReader _resourceReader;

    private const string ConfigurationProdFileName = "Configuration.Production.cfg.xml";
    private const string ConfigurationDevFileName = "Configuration.Development.cfg.xml";
    private const string ConfigurationFileName = "Configuration.cfg.xml";

    public ConfigurationLocationProvider(IResourceReader resourceReader)
    {
        _resourceReader = resourceReader;
    }

    public async Task<string> GetOrCreateAsync()
    {
        var exeLocation = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\{ConfigurationFileName}";
        var appDataLocation = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\TypeCode\{ConfigurationFileName}";

        if (File.Exists(exeLocation))
        {
            return exeLocation;
        }

        if (File.Exists(appDataLocation))
        {
            return appDataLocation;
        }

#if DEBUG
        var configTemplate = _resourceReader.ReadResource(Assembly.GetExecutingAssembly(), $"Configuration.Location.{ConfigurationDevFileName}");
#else
        var configTemplate = _resourceReader.ReadResource(Assembly.GetExecutingAssembly(), $"Configuration.Location.{ConfigurationProdFileName}");
#endif

        Directory.CreateDirectory(Path.GetDirectoryName(appDataLocation) ?? throw new ArgumentException($"{appDataLocation} is not valid location"));
        await File.WriteAllTextAsync(appDataLocation, configTemplate).ConfigureAwait(false);
        return appDataLocation;
    }
}