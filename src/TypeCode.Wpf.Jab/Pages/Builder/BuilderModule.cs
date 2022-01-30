using Autofac;
using TypeCode.Wpf.Jab.Helper.Autofac;

namespace TypeCode.Wpf.Jab.Pages.Builder;

public class BuilderModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.AddViewModelAndView<BuilderViewModel, BuilderView>();

        base.Load(builder);
    }
}