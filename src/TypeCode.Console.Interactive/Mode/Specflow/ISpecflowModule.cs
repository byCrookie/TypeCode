using Jab;
using TypeCode.Console.Interactive.Mode.ExitOrContinue;

namespace TypeCode.Console.Interactive.Mode.Specflow;

[ServiceProviderModule]
[Transient(typeof(ISpecflowTypeCodeStrategy), typeof(SpecflowTypeCodeStrategy))]
[Transient(typeof(IExitOrContinueStep<SpecflowContext>), typeof(ExitOrContinueStep<SpecflowContext>))]
public interface ISpecflowModule
{
    
}