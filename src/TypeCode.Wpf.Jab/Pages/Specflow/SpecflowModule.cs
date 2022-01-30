using Autofac;
using TypeCode.Wpf.Jab.Helper.Autofac;

namespace TypeCode.Wpf.Jab.Pages.Specflow;

public class SpecflowModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.AddViewModelAndView<SpecflowViewModel, SpecflowView>();

        base.Load(builder);
    }
}