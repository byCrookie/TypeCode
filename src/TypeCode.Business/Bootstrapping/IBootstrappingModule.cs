using Framework.Jab.Boot;
using Jab;

namespace TypeCode.Business.Bootstrapping;

[ServiceProviderModule]
[Transient(typeof(IConfigurationLoadBootStep<BootContext>), typeof(ConfigurationLoadBootStep<BootContext>))]
public interface IBootstrappingModule
{
    
}