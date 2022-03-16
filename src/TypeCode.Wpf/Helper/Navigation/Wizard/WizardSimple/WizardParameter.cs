using TypeCode.Wpf.Helper.Navigation.Service;

namespace TypeCode.Wpf.Helper.Navigation.Wizard.WizardSimple;

public class WizardParameter<T>
{
    public WizardParameter()
    {
        CanSave = _ => true;
        OnSaveAsync = (_, _) => Task.CompletedTask;
        OnCancelAsync = (_, _) => Task.CompletedTask;
    }
    
    public string? FinishButtonText { get; set; }
    public Func<T, bool> CanSave { get; set; }
    public Func<T, NavigationContext, Task> OnSaveAsync { get; set; }
    public Func<T, NavigationContext, Task> OnCancelAsync { get; set; }
}