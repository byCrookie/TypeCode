namespace TypeCode.Business.Bootstrapping.Data;

public interface IUserDataLocationInitializer
{
    void InitializeConfigurationFilePath(string location);
    void InitializeLogsPath(string location);
    void InitializeCachePath(string location);
}