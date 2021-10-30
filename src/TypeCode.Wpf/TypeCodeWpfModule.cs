using Autofac;
using Framework.Boot;
using TypeCode.Wpf.Builder;
using TypeCode.Wpf.Composer;
using TypeCode.Wpf.Helper;
using TypeCode.Wpf.Helper.Boot;
using TypeCode.Wpf.Mapper;
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
            builder.RegisterModule<ComposerModule>();
            builder.RegisterModule<MapperModule>();
            builder.RegisterModule<BuilderModule>();
            
            builder.RegisterModule<HelperModule>();
            builder.RegisterModule<BootModule>();
            
            base.Load(builder);
        }
    }
}