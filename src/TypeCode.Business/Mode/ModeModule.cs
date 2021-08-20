using Autofac;
using TypeCode.Business.Mode.Builder;
using TypeCode.Business.Mode.Composer;
using TypeCode.Business.Mode.Mapper;
using TypeCode.Business.Mode.MultipleTypes;
using TypeCode.Business.Mode.Selection;
using TypeCode.Business.Mode.Specflow;
using TypeCode.Business.Mode.UnitTestDependency;

namespace TypeCode.Business.Mode
{
    internal class ModeModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SpecflowTypeCodeStrategy>().As<ISpecflowTypeCodeStrategy>();
            builder.RegisterType<BuilderTypeCodeStrategy>().As<IBuilderTypeCodeStrategy>();
            builder.RegisterType<UnitTestDependencyTypeCodeStrategy>().As<IUnitTestDependencyTypeCodeStrategy>();
            builder.RegisterType<ComposerTypeCodeStrategy>().As<IComposerTypeCodeStrategy>();
            
            builder.RegisterType<ModeComposer>().As<IModeComposer>();

            builder.RegisterModule<MapperModule>();
            builder.RegisterModule<MultipleTypesModule>();
            builder.RegisterModule<SelectionModule>();
            
            base.Load(builder);
        }
    }
}