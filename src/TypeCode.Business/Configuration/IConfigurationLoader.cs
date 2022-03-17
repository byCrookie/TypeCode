namespace TypeCode.Business.Configuration;

public interface IConfigurationLoader
{
    public Task<TypeCodeConfiguration> LoadAsync();
}