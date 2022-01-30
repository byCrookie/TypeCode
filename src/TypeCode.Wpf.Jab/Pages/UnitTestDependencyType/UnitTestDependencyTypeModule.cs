using Autofac;
using TypeCode.Wpf.Jab.Helper.Autofac;

namespace TypeCode.Wpf.Jab.Pages.UnitTestDependencyType;

public class UnitTestDependencyTypeModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.AddViewModelAndView<UnitTestDependencyTypeViewModel, UnitTestDependencyTypeView>();

        base.Load(builder);
    }
}