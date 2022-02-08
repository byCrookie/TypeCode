using Autofac;
using TypeCode.Console.Interactive.Mode.Builder;
using TypeCode.Console.Interactive.Mode.Composer;
using TypeCode.Console.Interactive.Mode.Exit;
using TypeCode.Console.Interactive.Mode.ExitOrContinue;
using TypeCode.Console.Interactive.Mode.Mapper;
using TypeCode.Console.Interactive.Mode.MultipleTypes;
using TypeCode.Console.Interactive.Mode.Selection;
using TypeCode.Console.Interactive.Mode.Specflow;
using TypeCode.Console.Interactive.Mode.UnitTestDependency;

namespace TypeCode.Console.Interactive.Mode;

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