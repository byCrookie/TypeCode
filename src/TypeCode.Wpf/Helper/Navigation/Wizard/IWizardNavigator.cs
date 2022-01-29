using System.Threading.Tasks;
using Workflow;

namespace TypeCode.Wpf.Helper.Navigation.Wizard
{
    public interface IWizardNavigator
    {
        Task<TContext> StartAsync<TContext>(WizardNavigatorParameter parameter, TContext context, IWorkflow<TContext> wizardFlow) where TContext : WizardContext;
        Task NextOrNewAsync<TViewModel>(WizardContext context);
        Task BackAsync(WizardContext context);
        Task CloseAsync(WizardContext context);
        Task CloseCurrentAsync(WizardContext context);
    }
}