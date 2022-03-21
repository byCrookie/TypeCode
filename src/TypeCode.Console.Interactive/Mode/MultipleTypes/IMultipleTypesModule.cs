using Jab;

namespace TypeCode.Console.Interactive.Mode.MultipleTypes;

[ServiceProviderModule]
[Transient(typeof(IMultipleTypeSelectionStep<>), typeof(MultipleTypeSelectionStep<>))]
public interface IMultipleTypesModule
{
}