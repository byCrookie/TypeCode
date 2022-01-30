using Autofac;
using TypeCode.Wpf.Jab.Pages.Assemblies;
using TypeCode.Wpf.Jab.Pages.Builder;
using TypeCode.Wpf.Jab.Pages.Common;
using TypeCode.Wpf.Jab.Pages.Composer;
using TypeCode.Wpf.Jab.Pages.Mapper;
using TypeCode.Wpf.Jab.Pages.Specflow;
using TypeCode.Wpf.Jab.Pages.TypeSelection;
using TypeCode.Wpf.Jab.Pages.UnitTestDependencyManually;
using TypeCode.Wpf.Jab.Pages.UnitTestDependencyType;

namespace TypeCode.Wpf.Jab.Pages;

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
        builder.RegisterModule<CommonWizardModule>();
            
        base.Load(builder);
    }
}