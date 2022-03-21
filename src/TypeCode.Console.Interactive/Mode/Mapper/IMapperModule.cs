using Jab;
using TypeCode.Console.Interactive.Mode.MultipleTypes;
using TypeCode.Console.Interactive.Mode.Selection;

namespace TypeCode.Console.Interactive.Mode.Mapper;

[ServiceProviderModule]
[Transient(typeof(IMapperTypeCodeStrategy), typeof(MapperTypeCodeStrategy))]
[Transient(typeof(IMultipleTypeSelectionStep<MappingContext>), typeof(MultipleTypeSelectionStep<MappingContext>))]
[Transient(typeof(ISelectionStep<MappingContext, SelectionStepOptions>), typeof(SelectionStep<MappingContext, SelectionStepOptions>))]
public interface IMapperModule
{
}