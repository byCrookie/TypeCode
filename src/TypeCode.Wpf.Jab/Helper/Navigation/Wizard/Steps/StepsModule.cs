using Autofac;
using TypeCode.Wpf.Jab.Helper.Navigation.Wizard.Steps.WizardEndStep;
using TypeCode.Wpf.Jab.Helper.Navigation.Wizard.Steps.WizardStep;

namespace TypeCode.Wpf.Jab.Helper.Navigation.Wizard.Steps;

public class StepsModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterModule<WizardStepModule>();
        builder.RegisterModule<WizardEndStepModule>();
            
        base.Load(builder);
    }
}