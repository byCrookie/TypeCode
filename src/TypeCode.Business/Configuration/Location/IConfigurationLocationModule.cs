using Jab;

namespace TypeCode.Business.Configuration.Location;

[ServiceProviderModule]
[Transient(typeof(IConfigurationLocationProvider), typeof(ConfigurationLocationProvider))]
public interface IConfigurationLocationModule
{
    
}