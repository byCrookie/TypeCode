using Framework.Boot;
using Framework.Boot.Start;
using Jab;
using TypeCode.Business.Modules;
using TypeCode.Wpf.Application.Boot.SetupWpfApplication;

namespace TypeCode.Wpf.Application.Boot;

[ServiceProviderModule]
[Import(typeof(ITypeCodeBusinessModule))]
[Transient(typeof(IStartBootStep<BootContext>), typeof(StartBootStep<BootContext>))]
[Transient(typeof(ISetupWpfApplicationStep<BootContext>), typeof(SetupWpfApplicationStep<BootContext>))]
public interface IBootModule
{
}