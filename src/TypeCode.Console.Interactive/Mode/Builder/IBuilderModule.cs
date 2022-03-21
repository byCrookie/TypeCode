using Jab;
using TypeCode.Console.Interactive.Mode.ExitOrContinue;
using TypeCode.Console.Interactive.Mode.MultipleTypes;

namespace TypeCode.Console.Interactive.Mode.Builder;

[ServiceProviderModule]
[Transient(typeof(IBuilderTypeCodeStrategy), typeof(BuilderTypeCodeStrategy))]
[Transient(typeof(IExitOrContinueStep<BuilderContext>), typeof(ExitOrContinueStep<BuilderContext>))]
[Transient(typeof(IMultipleTypeSelectionStep<BuilderContext>), typeof(MultipleTypeSelectionStep<BuilderContext>))]
public interface IBuilderModule
{
    
}