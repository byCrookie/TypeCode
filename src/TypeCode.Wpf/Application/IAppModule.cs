using Framework.Boot;
using Jab;
using TypeCode.Wpf.Application.Boot;

namespace TypeCode.Wpf.Application;

[ServiceProviderModule]
[Import(typeof(IBootModule))]
[Transient(typeof(IApplication<BootContext>), typeof(Application<BootContext>))]
public interface IAppModule
{
}