using TypeCode.Wpf.Helper.Navigation.Service;

namespace TypeCode.Wpf.Helper.Navigation.Wizard.Complex;

public class WizardStepParameter
{
    public WizardStepParameter()
    {
        BeforeAction = _ => Task.CompletedTask;
        AfterAction = _ => Task.CompletedTask;
        AllowBack = _ => false;
        AllowNext = _ => false;
    }
        
    public Func<NavigationContext, Task> BeforeAction { get; set; }
    public Func<NavigationContext, Task> AfterAction { get; set; }
    public Func<NavigationContext, bool> AllowBack { get; set; }
    public Func<NavigationContext, bool> AllowNext { get; set; }
}