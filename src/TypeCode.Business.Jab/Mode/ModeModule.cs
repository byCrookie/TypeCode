using Autofac;
using TypeCode.Business.Mode.Builder;
using TypeCode.Business.Mode.Composer;
using TypeCode.Business.Mode.Mapper;
using TypeCode.Business.Mode.Mapper.Style;
using TypeCode.Business.Mode.Specflow;
using TypeCode.Business.Mode.UnitTestDependency.Manually;
using TypeCode.Business.Mode.UnitTestDependency.Type;

namespace TypeCode.Business.Mode;

internal class ModeModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<SpecflowTypeCodeGenerator>().As<ITypeCodeGenerator<SpecflowTypeCodeGeneratorParameter>>();
        builder.RegisterType<BuilderTypeCodeGenerator>().As<ITypeCodeGenerator<BuilderTypeCodeGeneratorParameter>>();
        builder.RegisterType<UnitTestDependencyManuallyTypeCodeGenerator>().As<ITypeCodeGenerator<UnitTestDependencyManuallyGeneratorParameter>>();
        builder.RegisterType<UnitTestDependencyTypeTypeCodeGenerator>().As<ITypeCodeGenerator<UnitTestDependencyTypeGeneratorParameter>>();
        builder.RegisterType<ComposerTypeCodeGenerator>().As<ITypeCodeGenerator<ComposerTypeCodeGeneratorParameter>>();
        builder.RegisterType<MapperTypeCodeGenerator>().As<ITypeCodeGenerator<MapperTypeCodeGeneratorParameter>>();
            
        builder.RegisterType<ExistingMapperStyleStrategy>().As<IExistingMapperStyleStrategy>();
        builder.RegisterType<NewMapperStyleStrategy>().As<INewMapperStyleStrategy>();
        builder.RegisterType<MapperStyleComposer>().As<IMapperStyleComposer>();

        base.Load(builder);
    }
}