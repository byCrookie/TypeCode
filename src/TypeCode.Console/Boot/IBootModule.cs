using Framework.Boot;
using Jab;

namespace TypeCode.Console.Boot;

[ServiceProviderModule]
[Transient(typeof(ITargetDllsLoadBootStep<BootContext, TargetDllsBootStepOptions>), typeof(TargetDllsLoadBootStep<BootContext, TargetDllsBootStepOptions>))]
public interface IBootModule
{
    
}