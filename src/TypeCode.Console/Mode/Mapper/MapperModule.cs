using Jab;
using Workflow;

namespace TypeCode.Console.Mode.Mapper;

[ServiceProviderModule]
[Transient(typeof(IWorkflowBuilder<MappingContext>), typeof(WorkflowBuilder<MappingContext>))]
[Transient(typeof(IMapperTypeCodeStrategy), typeof(MapperTypeCodeStrategy))]
internal partial interface IMapperModule
{
}