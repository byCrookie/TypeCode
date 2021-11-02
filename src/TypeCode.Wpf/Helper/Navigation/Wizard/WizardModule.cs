using Autofac;
using TypeCode.Wpf.Helper.Navigation.Wizard.Steps;
using TypeCode.Wpf.Helper.Navigation.Wizard.View;
using TypeCode.Wpf.Helper.Navigation.Wizard.WizardSimple;

namespace TypeCode.Wpf.Helper.Navigation.Wizard
{
    public class WizardModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<WizardNavigator>().As<IWizardNavigator>();
            builder.RegisterType<WizardViewModel>().AsSelf();
            
            builder.RegisterType<WizardNavigationService>().As<IWizardNavigationService>();
            builder.RegisterGeneric(typeof(WizardSimpleViewModel<>)).AsSelf();

            builder.RegisterModule<StepsModule>();

            base.Load(builder);
        }
    }
}