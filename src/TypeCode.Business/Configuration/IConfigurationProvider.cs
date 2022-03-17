namespace TypeCode.Business.Configuration;

public interface IConfigurationProvider
{
    void SetConfiguration(TypeCodeConfiguration configuration);
    TypeCodeConfiguration GetConfiguration();
}