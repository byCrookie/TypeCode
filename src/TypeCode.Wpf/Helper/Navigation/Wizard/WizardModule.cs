using Autofac;
using TypeCode.Wpf.Helper.Autofac;
using TypeCode.Wpf.Helper.Navigation.Wizard.Steps;
using TypeCode.Wpf.Helper.Navigation.Wizard.WizardSimple;
using WizardSimpleView = TypeCode.Wpf.Helper.Navigation.Wizard.WizardSimple.WizardSimpleView;

namespace TypeCode.Wpf.Helper.Navigation.Wizard;

public class WizardModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<WizardNavigator>().As<IWizardNavigator>();

        builder.RegisterType<WizardNavigationService>().As<IWizardNavigationService>();
        builder.AddGenericViewModelAndView(typeof(WizardSimpleViewModel<>), typeof(WizardSimpleView));

        builder.RegisterModule<StepsModule>();

        base.Load(builder);
    }
}