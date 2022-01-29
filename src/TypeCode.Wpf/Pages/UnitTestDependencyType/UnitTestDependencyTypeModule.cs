using Autofac;
using TypeCode.Wpf.Helper.Autofac;

namespace TypeCode.Wpf.Pages.UnitTestDependencyType;

public class UnitTestDependencyTypeModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.AddViewModelAndView<UnitTestDependencyTypeViewModel, UnitTestDependencyTypeView>();

        base.Load(builder);
    }
}