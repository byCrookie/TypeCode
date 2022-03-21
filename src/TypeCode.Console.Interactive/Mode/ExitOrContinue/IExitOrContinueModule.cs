using Jab;

namespace TypeCode.Console.Interactive.Mode.ExitOrContinue;

[ServiceProviderModule]
[Transient(typeof(IExitOrContinueStep<TypeCodeContext>), typeof(ExitOrContinueStep<TypeCodeContext>))]
public interface IExitOrContinueModule
{
}