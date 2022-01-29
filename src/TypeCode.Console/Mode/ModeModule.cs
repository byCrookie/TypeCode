using Autofac;
using TypeCode.Console.Mode.Builder;
using TypeCode.Console.Mode.Composer;
using TypeCode.Console.Mode.Exit;
using TypeCode.Console.Mode.ExitOrContinue;
using TypeCode.Console.Mode.Mapper;
using TypeCode.Console.Mode.MultipleTypes;
using TypeCode.Console.Mode.Selection;
using TypeCode.Console.Mode.Specflow;
using TypeCode.Console.Mode.UnitTestDependency;

namespace TypeCode.Console.Mode;

internal class ModeModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<SpecflowTypeCodeStrategy>().As<ISpecflowTypeCodeStrategy>();
        builder.RegisterType<BuilderTypeCodeStrategy>().As<IBuilderTypeCodeStrategy>();
        builder.RegisterType<UnitTestDependencyTypeCodeStrategy>().As<IUnitTestDependencyTypeCodeStrategy>();
        builder.RegisterType<ComposerTypeCodeStrategy>().As<IComposerTypeCodeStrategy>();
        builder.RegisterType<ExitTypeCodeStrategy>().As<IExitTypeCodeStrategy>();
            
        builder.RegisterType<ModeComposer>().As<IModeComposer>();

        builder.RegisterModule<MapperModule>();
        builder.RegisterModule<MultipleTypesModule>();
        builder.RegisterModule<SelectionModule>();
        builder.RegisterModule<ExitOrContinueModule>();
            
        base.Load(builder);
    }
}