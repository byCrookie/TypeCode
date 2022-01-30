using Autofac;
using TypeCode.Wpf.Jab.Helper.Autofac;

namespace TypeCode.Wpf.Jab.Pages.TypeSelection;

public class TypeSelectionModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.AddViewModelAndView<TypeSelectionViewModel, TypeSelectionView>();

        base.Load(builder);
    }
}