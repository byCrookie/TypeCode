using Autofac;

namespace TypeCode.Wpf.UnitTestDependencyType
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