namespace TypeCode.Wpf.Helper.Navigation.Wizard.WizardSimple;

public class WizardParameter<T>
{
    public WizardParameter()
    {
        OnCloseAsync = _ => Task.CompletedTask;
    }
    
    public string? FinishButtonText { get; set; }
    public Func<T, Task> OnCloseAsync { get; set; }
}