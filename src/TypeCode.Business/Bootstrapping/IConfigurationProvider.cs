using TypeCode.Business.Configuration;

namespace TypeCode.Business.Bootstrapping
{
    public interface IConfigurationProvider
    {
        void SetConfiguration(TypeCodeConfiguration configuration);
        TypeCodeConfiguration GetConfiguration();
    }
}