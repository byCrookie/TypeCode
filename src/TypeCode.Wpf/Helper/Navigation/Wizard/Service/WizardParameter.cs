using System;
using System.Threading.Tasks;

namespace TypeCode.Wpf.Helper.Navigation.Wizard.Service
{
    public class WizardParameter<T>
    {
        public string FinishButtonText { get; set; }
        public Func<T, Task> OnCloseAsync { get; set; }
    }
}