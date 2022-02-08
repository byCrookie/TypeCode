using Autofac;
using Framework.Autofac.Boot;
using TypeCode.Console.Interactive.Mode;

namespace TypeCode.Console.Interactive.Modules;

internal class TypeCodeConsoleModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterGeneric(typeof(TypeCode<>)).As(typeof(IApplication<>));
        builder.RegisterModule<ModeModule>();

        base.Load(builder);
    }
}