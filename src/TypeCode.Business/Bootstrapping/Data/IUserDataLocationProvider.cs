namespace TypeCode.Business.Bootstrapping.Data;

public interface IUserDataLocationProvider
{
    string GetConfigurationPath();
    string GetLogsPath();
}