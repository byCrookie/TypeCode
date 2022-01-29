using Autofac;

namespace TypeCode.Wpf.Helper.Navigation.Wizard.Steps.WizardEndStep;

internal class WizardEndStepModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterGeneric(typeof(WizardEndStep<,>)).As(typeof(IWizardEndStep<,>));

        base.Load(builder);
    }
}