using Jab;

namespace TypeCode.Console.Mode.Selection;

[ServiceProviderModule]
[Transient(typeof(ISelectionStep<,>), typeof(SelectionStep<,>))]
internal partial interface ISelectionModule
{
}