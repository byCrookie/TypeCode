using Jab;
using TypeCode.Business.Mode.Builder;
using TypeCode.Business.Mode.Composer;
using TypeCode.Business.Mode.DynamicExecution;
using TypeCode.Business.Mode.EncodingConversion;
using TypeCode.Business.Mode.Guid;
using TypeCode.Business.Mode.Mapper;
using TypeCode.Business.Mode.Mapper.Style;
using TypeCode.Business.Mode.Specflow;
using TypeCode.Business.Mode.UnitTestDependency.Manually;
using TypeCode.Business.Mode.UnitTestDependency.Type;

namespace TypeCode.Business.Mode;

[ServiceProviderModule]
[Transient(typeof(ITypeCodeGenerator<SpecflowTypeCodeGeneratorParameter>), typeof(SpecflowTypeCodeGenerator))]
[Transient(typeof(ITypeCodeGenerator<BuilderTypeCodeGeneratorParameter>), typeof(BuilderTypeCodeGenerator))]
[Transient(typeof(ITypeCodeGenerator<UnitTestDependencyManuallyGeneratorParameter>), typeof(UnitTestDependencyManuallyTypeCodeGenerator))]
[Transient(typeof(ITypeCodeGenerator<UnitTestDependencyTypeGeneratorParameter>), typeof(UnitTestDependencyTypeTypeCodeGenerator))]
[Transient(typeof(ITypeCodeGenerator<ComposerTypeCodeGeneratorParameter>), typeof(ComposerTypeCodeGenerator))]
[Transient(typeof(ITypeCodeGenerator<MapperTypeCodeGeneratorParameter>), typeof(MapperTypeCodeGenerator))]
[Transient(typeof(ITypeCodeGenerator<GuidTypeCodeGeneratorParameter>), typeof(GuidCodeGenerator))]
[Transient(typeof(ITypeCodeGenerator<EncodingTypeCodeGeneratorParameter>), typeof(EncodingTypeCodeGenerator))]
[Transient(typeof(IExistingMapperStyleStrategy), typeof(ExistingMapperStyleStrategy))]
[Transient(typeof(INewMapperStyleStrategy), typeof(NewMapperStyleStrategy))]
[Transient(typeof(IMapperStyleComposer), typeof(MapperStyleComposer))]
[Transient(typeof(ITableFormatter), typeof(TableFormatter))]
[Import(typeof(IDynamicExecutionModule))]
public interface IModeModule
{
}