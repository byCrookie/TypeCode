using Autofac;
using TypeCode.Wpf.Helper.Navigation.Modal;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.Navigation.Wizard;
using TypeCode.Wpf.Helper.Navigation.Wizard.Steps;

namespace TypeCode.Wpf.Helper.Navigation
{
    public class NavigationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<NavigationService>().As<INavigationService>().SingleInstance();
            builder.RegisterType<WizardNavigator>().As<IWizardNavigator>();

            builder.RegisterModule<ModalModule>();
            builder.RegisterModule<WizardModule>();
            
            base.Load(builder);
        }
    }
}