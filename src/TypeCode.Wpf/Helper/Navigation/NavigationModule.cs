using Autofac;
using TypeCode.Wpf.Helper.Navigation.Modal;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.Navigation.Wizard;
using TypeCode.Wpf.Helper.Navigation.Wizard.Complex;
using TypeCode.Wpf.Helper.Navigation.Wizard.Steps;
using IWizardNavigator = TypeCode.Wpf.Helper.Navigation.Wizard.IWizardNavigator;
using WizardNavigator = TypeCode.Wpf.Helper.Navigation.Wizard.WizardNavigator;

namespace TypeCode.Wpf.Helper.Navigation
{
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
}