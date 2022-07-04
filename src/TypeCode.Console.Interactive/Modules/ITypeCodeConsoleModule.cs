using Framework.Boot;
using Framework.Boot.Start;
using Jab;
using TypeCode.Console.Interactive.Mode;

namespace TypeCode.Console.Interactive.Modules;

[ServiceProviderModule]
[Import(typeof(IModeModule))]
[Transient(typeof(IStartBootStep<BootContext>), typeof(StartBootStep<BootContext>))]
[Transient(typeof(IApplication<BootContext>), typeof(TypeCode<BootContext>))]
public interface ITypeCodeConsoleModule
{
}