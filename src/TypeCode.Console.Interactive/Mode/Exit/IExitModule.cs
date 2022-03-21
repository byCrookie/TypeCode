using Jab;

namespace TypeCode.Console.Interactive.Mode.Exit;

[ServiceProviderModule]
[Transient(typeof(IExitTypeCodeStrategy), typeof(ExitTypeCodeStrategy))]
public interface IExitModule
{
    
}