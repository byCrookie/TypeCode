using Autofac;
using TypeCode.Wpf.Jab.Helper.Autofac;
using TypeCode.Wpf.Jab.Helper.Navigation.Wizard.Steps;
using TypeCode.Wpf.Jab.Helper.Navigation.Wizard.View;
using TypeCode.Wpf.Jab.Helper.Navigation.Wizard.WizardSimple;
using WizardSimpleView = TypeCode.Wpf.Jab.Helper.Navigation.Wizard.WizardSimple.WizardSimpleView;
using WizardView = TypeCode.Wpf.Jab.Helper.Navigation.Wizard.Complex.WizardView;

namespace TypeCode.Wpf.Jab.Helper.Navigation.Wizard;

public class WizardModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<WizardNavigator>().As<IWizardNavigator>();
        builder.AddViewModelAndView<WizardSimpleViewModel, WizardView>();
            
        builder.RegisterType<WizardNavigationService>().As<IWizardNavigationService>();
        builder.AddGenericViewModelAndView(typeof(WizardSimpleViewModel<>), typeof(WizardSimpleView));

        builder.RegisterModule<StepsModule>();

        base.Load(builder);
    }
}