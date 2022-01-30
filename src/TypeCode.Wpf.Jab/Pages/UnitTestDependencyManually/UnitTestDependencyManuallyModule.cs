using Autofac;
using TypeCode.Wpf.Jab.Helper.Autofac;

namespace TypeCode.Wpf.Jab.Pages.UnitTestDependencyManually;

public class UnitTestDependencyManuallyModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.AddViewModelAndView<UnitTestDependencyManuallyViewModel, UnitTestDependencyManuallyView>();

        base.Load(builder);
    }
}