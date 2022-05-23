namespace TypeCode.Business.Configuration.Location;

public class ConfigurationLocationProvider : IConfigurationLocationProvider
{
    private const string ConfigurationFileName = "Configuration.cfg.xml";

    public string GetLocation()
    {
        var exeLocation = $@"{Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)}\{ConfigurationFileName}";
        var appDataLocation = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\TypeCode\{ConfigurationFileName}";

        return File.Exists(exeLocation)
            ? exeLocation
            : appDataLocation;
    }
}