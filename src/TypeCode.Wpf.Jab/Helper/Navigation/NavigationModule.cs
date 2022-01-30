using Autofac;
using TypeCode.Wpf.Jab.Helper.Navigation.Modal;
using TypeCode.Wpf.Jab.Helper.Navigation.Service;
using TypeCode.Wpf.Jab.Helper.Navigation.Wizard;
using TypeCode.Wpf.Jab.Helper.Navigation.Wizard.Complex;

namespace TypeCode.Wpf.Jab.Helper.Navigation;

public class NavigationModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<NavigationService>().As<INavigationService>().SingleInstance();

        builder.RegisterModule<ModalModule>();
        builder.RegisterModule<WizardModule>();
        builder.RegisterModule<WizardComplexModule>();
            
        base.Load(builder);
    }
}