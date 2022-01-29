using Autofac;
using TypeCode.Wpf.Helper.Autofac;

namespace TypeCode.Wpf.Pages.Builder;

public class BuilderModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.AddViewModelAndView<BuilderViewModel, BuilderView>();

        base.Load(builder);
    }
}