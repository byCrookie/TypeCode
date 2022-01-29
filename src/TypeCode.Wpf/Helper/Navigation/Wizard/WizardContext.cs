using TypeCode.Wpf.Helper.Navigation.Service;
using Workflow;

namespace TypeCode.Wpf.Helper.Navigation.Wizard
{
    public class WizardContext : WorkflowBaseContext
    {
        public WizardContext(NavigationContext navigationContext = null)
        {
            NavigationJournal = new NavigationJournal();
            NavigationContext = navigationContext ?? new NavigationContext();
        }
        
        public NavigationJournal NavigationJournal { get; set; }
        public NavigationContext NavigationContext { get; set; }
        public InstanceResult Wizard { get; set; }
    }
}