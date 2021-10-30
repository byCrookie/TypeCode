using Autofac;

namespace TypeCode.Wpf.UnitTestDependencyManually
{
    public class UnitTestDependencyManuallyModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UnitTestDependencyManuallyViewModel>().AsSelf();
            
            base.Load(builder);
        }
    }
}