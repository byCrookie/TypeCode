using TypeCode.Wpf.Helper.Navigation.Service;

namespace TypeCode.Wpf.Helper.Navigation.Wizard.WizardSimple;

public class WizardParameter<T>
{
    public WizardParameter()
    {
        OnSaveAsync = _ => Task.CompletedTask;
        OnCancelAsync = _ => Task.CompletedTask;
    }
    
    public string? FinishButtonText { get; set; }
    public Func<NavigationContext, Task> OnSaveAsync { get; set; }
    public Func<NavigationContext, Task> OnCancelAsync { get; set; }
}