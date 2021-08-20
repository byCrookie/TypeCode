using TypeCode.Business.Configuration;

namespace TypeCode.Business.Bootstrapping
{
    internal interface IConfigurationProvider
    {
        void SetConfiguration(TypeCodeConfiguration configuration);
        TypeCodeConfiguration GetConfiguration();
    }
}