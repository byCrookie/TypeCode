using Autofac;
using TypeCode.Wpf.Pages.Assemblies;
using TypeCode.Wpf.Pages.Builder;
using TypeCode.Wpf.Pages.Composer;
using TypeCode.Wpf.Pages.Mapper;
using TypeCode.Wpf.Pages.Specflow;
using TypeCode.Wpf.Pages.TypeSelection;
using TypeCode.Wpf.Pages.UnitTestDependencyManually;
using TypeCode.Wpf.Pages.UnitTestDependencyType;

namespace TypeCode.Wpf.Pages
{
    public class PagesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<SpecflowModule>();
            builder.RegisterModule<UnitTestDependencyTypeModule>();
            builder.RegisterModule<UnitTestDependencyManuallyModule>();
            builder.RegisterModule<ComposerModule>();
            builder.RegisterModule<MapperModule>();
            builder.RegisterModule<BuilderModule>();
            
            builder.RegisterModule<AssemblyModule>();

            builder.RegisterModule<TypeSelectionModule>();
            
            base.Load(builder);
        }
    }
}