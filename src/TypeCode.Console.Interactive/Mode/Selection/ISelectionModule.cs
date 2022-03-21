using Jab;
using TypeCode.Console.Interactive.Mode.ExitOrContinue;

namespace TypeCode.Console.Interactive.Mode.Selection;

[ServiceProviderModule]
[Transient(typeof(ISelectionStep<,>), typeof(SelectionStep<,>))]
[Transient(typeof(IExitOrContinueStep<SelectionContext>), typeof(ExitOrContinueStep<SelectionContext>))]
public interface ISelectionModule
{
}