using Framework.Jab.Boot;
using Jab;

namespace TypeCode.Business.Bootstrapping;

[ServiceProviderModule]
[Transient(typeof(IConfigurationJabLoadBootStep<BootContext>), typeof(ConfigurationJabLoadBootStep<BootContext>))]
public interface IBootstrappingModule
{
    
}