namespace TypeCode.Business.Bootstrapping.Data;

public interface IUserDataLocationProvider
{
    string GetConfigurationFilePath();
    string GetLogsPath();
    string GetCachePath();
    string GetDynamicExecutionPath();
}