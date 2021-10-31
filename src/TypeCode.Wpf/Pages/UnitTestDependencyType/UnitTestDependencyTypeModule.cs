using Autofac;

namespace TypeCode.Wpf.Pages.UnitTestDependencyType
{
    public class UnitTestDependencyTypeModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UnitTestDependencyTypeViewModel>().AsSelf();
            
            base.Load(builder);
        }
    }
}