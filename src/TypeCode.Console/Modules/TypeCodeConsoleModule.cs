using Autofac;
using Framework.Autofac.Boot;
using TypeCode.Console.Mode;

namespace TypeCode.Console.Modules
{
    internal class TypeCodeConsoleModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TypeCode>().As<IApplication>();
            builder.RegisterModule<ModeModule>();

            base.Load(builder);
        }
    }
}