using Framework.Boot;
using Jab;
using TypeCode.Business.Bootstrapping.Configuration;
using TypeCode.Business.Bootstrapping.Data;

namespace TypeCode.Business.Bootstrapping;

[ServiceProviderModule]
[Transient(typeof(IConfigurationLoadBootStep<BootContext>), typeof(ConfigurationLoadBootStep<BootContext>))]
[Transient(typeof(IUserDataInitializeBootStep<BootContext>), typeof(UserDataInitializeBootStep<BootContext>))]
[Singleton(typeof(IUserDataLocationProvider), typeof(UserDataLocationProvider))]
public interface IBootstrappingModule
{
    
}