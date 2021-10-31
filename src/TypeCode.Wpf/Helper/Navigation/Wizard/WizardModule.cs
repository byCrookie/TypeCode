using Autofac;
using TypeCode.Wpf.Helper.Navigation.Wizard.Service;
using TypeCode.Wpf.Helper.Navigation.Wizard.View;

namespace TypeCode.Wpf.Helper.Navigation.Wizard
{
    public class WizardModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<WizardNavigationService>().As<IWizardNavigationService>().SingleInstance();
            
            builder.RegisterGeneric(typeof(WizardViewModel<>)).AsSelf();
            
            base.Load(builder);
        }
    }
}