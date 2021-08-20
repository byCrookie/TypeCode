using Autofac;
using Framework.Boot;

namespace TypeCode.Console
{
    public class TypeCodeConsoleModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Application>().As<IApplication>();
            
            base.Load(builder);
        }
    }
}