using Autofac;
using TypeCode.Wpf.Helper.Autofac;

namespace TypeCode.Wpf.Pages.Composer;

public class ComposerModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.AddViewModelAndView<ComposerViewModel, ComposerView>();

        base.Load(builder);
    }
}