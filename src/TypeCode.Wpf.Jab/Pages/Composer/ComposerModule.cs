using Autofac;
using TypeCode.Wpf.Jab.Helper.Autofac;

namespace TypeCode.Wpf.Jab.Pages.Composer;

public class ComposerModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.AddViewModelAndView<ComposerViewModel, ComposerView>();

        base.Load(builder);
    }
}