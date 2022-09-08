namespace TypeCode.Business.Bootstrapping.Data;

public interface IUserDataLocationInitializer
{
    void InitializeConfigurationPath(string location);
    void InitializeLogsPath(string location);
    void InitializeCachePath(string location);
}