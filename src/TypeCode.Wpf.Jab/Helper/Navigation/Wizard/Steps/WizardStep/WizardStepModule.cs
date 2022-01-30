using Autofac;

namespace TypeCode.Wpf.Jab.Helper.Navigation.Wizard.Steps.WizardStep;

internal class WizardStepModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterGeneric(typeof(WizardStep<,,>)).As(typeof(IWizardStep<,,>));

        base.Load(builder);
    }
}