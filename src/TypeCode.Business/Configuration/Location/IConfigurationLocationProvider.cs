namespace TypeCode.Business.Configuration.Location;

public interface IConfigurationLocationProvider
{
    Task<string> GetOrCreateAsync();
}