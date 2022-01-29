using Jab;

namespace TypeCode.Console.Mode.ExitOrContinue;

[ServiceProviderModule]
[Transient(typeof(IExitOrContinueStep<>), typeof(ExitOrContinueStep<>))]
internal partial interface IExitOrContinueModule
{
}