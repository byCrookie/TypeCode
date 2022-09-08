namespace TypeCode.Business.Bootstrapping.Data;

public class UserDataLocationProvider : IUserDataLocationProvider, IUserDataLocationInitializer
{
    private static string? _configurationPath;
    private static string? _logsPath;
    private static string? _cachePath;

    public string GetConfigurationPath()
    {
        if (_configurationPath is null)
        {
            throw new Exception($"{nameof(UserDataLocationProvider)} not initialized");
        }

        return _configurationPath;
    }

    public string GetLogsPath()
    {
        if (_logsPath is null)
        {
            throw new Exception($"{nameof(UserDataLocationProvider)} not initialized");
        }

        return _logsPath;
    }
    
    public string GetCachePath()
    {
        if (_cachePath is null)
        {
            throw new Exception($"{nameof(UserDataLocationProvider)} not initialized");
        }

        return _cachePath;
    }

    public void InitializeConfigurationPath(string location)
    {
        _configurationPath = location;
    }

    public void InitializeLogsPath(string location)
    {
        _logsPath = location;
    }
    
    public void InitializeCachePath(string location)
    {
        _cachePath = location;
    }
}