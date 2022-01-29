using Jab;

namespace TypeCode.Console.Mode.MultipleTypes;

[ServiceProviderModule]
[Transient(typeof(IMultipleTypeSelectionStep<>), typeof(MultipleTypeSelectionStep<>))]
internal partial interface IMultipleTypesModule
{
}