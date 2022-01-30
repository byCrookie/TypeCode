using Workflow;

namespace TypeCode.Wpf.Jab.Helper.Navigation.Wizard.Steps.WizardStep;

internal interface IWizardStep<in TWizardPage, in TContext, in TOptions> : 
    IWorkflowOptionsStep<TContext, TOptions>
    where TContext : WizardContext
{
}