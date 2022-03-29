namespace TypeCode.Business.Configuration;

public interface IConfigurationProvider
{
    void Set(TypeCodeConfiguration configuration);
    TypeCodeConfiguration Get();
    bool IsSet();
}