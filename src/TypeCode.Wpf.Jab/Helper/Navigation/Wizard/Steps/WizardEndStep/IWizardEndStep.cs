using Workflow;

namespace TypeCode.Wpf.Jab.Helper.Navigation.Wizard.Steps.WizardEndStep;

internal interface IWizardEndStep<in TContext, in TOptions> : 
    IWorkflowOptionsStep<TContext, TOptions>
    where TContext : WizardContext
{
}