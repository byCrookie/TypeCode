using Autofac;
using Framework.Boot;
using TypeCode.Wpf.Helper;
using TypeCode.Wpf.Helper.Boot;
using TypeCode.Wpf.Specflow;
using TypeCode.Wpf.UnitTestDependencyManually;
using TypeCode.Wpf.UnitTestDependencyType;

namespace TypeCode.Wpf
{
    public class TypeCodeWpfModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Application>().As<IApplication>();
            
            builder.RegisterModule<SpecflowModule>();
            builder.RegisterModule<UnitTestDependencyTypeModule>();
            builder.RegisterModule<UnitTestDependencyManuallyModule>();
            builder.RegisterModule<HelperModule>();
            builder.RegisterModule<BootModule>();
            
            base.Load(builder);
        }
    }
}