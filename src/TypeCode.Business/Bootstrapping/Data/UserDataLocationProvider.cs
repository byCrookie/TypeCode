namespace TypeCode.Business.Bootstrapping.Data;

public sealed class UserDataLocationProvider : IUserDataLocationProvider, IUserDataLocationInitializer
{
    private static string? _configurationFilePath;
    private static string? _logsPath;
    private static string? _cachePath;
    private static string? _dynamicExecutionPath;

    public string GetConfigurationFilePath()
    {
        if (_configurationFilePath is null)
        {
            throw new Exception($"{nameof(UserDataLocationProvider)} not initialized");
        }

        return _configurationFilePath;
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
    
    
    public string GetDynamicExecutionPath()
    {
        if (_dynamicExecutionPath is null)
        {
            throw new Exception($"{nameof(UserDataLocationProvider)} not initialized");
        }

        return _dynamicExecutionPath;
    }

    public void InitializeConfigurationFilePath(string location)
    {
        _configurationFilePath = location;
    }

    public void InitializeLogsPath(string location)
    {
        _logsPath = location;
    }
    
    public void InitializeCachePath(string location)
    {
        _cachePath = location;
    }
    
    public void InitializeDynamicExecutionPath(string location)
    {
        _dynamicExecutionPath = location;
    }
}