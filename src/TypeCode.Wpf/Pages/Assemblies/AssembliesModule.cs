using Autofac;

namespace TypeCode.Wpf.Pages.Assemblies
{
    public class AssemblyModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AssemblyViewModel>().AsSelf();
            
            base.Load(builder);
        }
    }
}