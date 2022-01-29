using Workflow;

namespace TypeCode.Wpf.Helper.Navigation.Wizard.Steps.WizardStep
{
    internal interface IWizardStep<in TWizardPage, in TContext, in TOptions> : 
        IWorkflowOptionsStep<TContext, TOptions>
        where TContext : WizardContext
    {
    }
}