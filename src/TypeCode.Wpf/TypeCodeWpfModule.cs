using Autofac;
using Framework.Boot;
using TypeCode.Wpf.Helper;
using TypeCode.Wpf.Helper.Boot;
using TypeCode.Wpf.Pages.Builder;
using TypeCode.Wpf.Pages.Composer;
using TypeCode.Wpf.Pages.Mapper;
using TypeCode.Wpf.Pages.Specflow;
using TypeCode.Wpf.Pages.UnitTestDependencyManually;
using TypeCode.Wpf.Pages.UnitTestDependencyType;

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