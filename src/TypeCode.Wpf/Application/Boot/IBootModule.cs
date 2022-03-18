using Framework.Jab.Boot;
using Framework.Jab.Boot.Start;
using Jab;
using TypeCode.Business.Modules;

namespace TypeCode.Wpf.Application.Boot;

[ServiceProviderModule]
[Import(typeof(ITypeCodeBusinessModule))]
[Transient(typeof(IStartBootStep<BootContext>), typeof(StartBootStep<BootContext>))]
[Transient(typeof(SetupWpfApplication.ISetupWpfApplicationStep<BootContext>), typeof(SetupWpfApplication.SetupWpfApplicationStep<BootContext>))]
public interface IBootModule
{
}