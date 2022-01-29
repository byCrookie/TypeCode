using Autofac;
using TypeCode.Wpf.Helper.Autofac;

namespace TypeCode.Wpf.Helper.Navigation.Wizard.Complex;

public class WizardComplexModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.AddViewModelAndView<WizardViewModel, WizardView>();

        builder.RegisterType<WizardBuilder>().As<IWizardBuilder>();
        builder.RegisterType<WizardNavigator>().As<IWizardNavigator>();
        builder.RegisterType<WizardRunner>().As<IWizardRunner>();
            
        base.Load(builder);
    }
}