using Autofac;
using TypeCode.Wpf.Helper.Autofac;

namespace TypeCode.Wpf.Pages.TypeSelection;

public class TypeSelectionModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.AddViewModelAndView<TypeSelectionViewModel, TypeSelectionView>();
        builder.RegisterType<TypeSelectionWizardStarter>().As<ITypeSelectionWizardStarter>();

        base.Load(builder);
    }
}