using System;
using System.Threading.Tasks;
using TypeCode.Wpf.Jab.Helper.Navigation.Service;

namespace TypeCode.Wpf.Jab.Helper.Navigation.Wizard.Complex;

public class WizardStepConfiguration
{
    public WizardStepConfiguration()
    {
        BeforeAction = _ => Task.CompletedTask;
        AfterAction = _ => Task.CompletedTask;
        AllowBack = _ => true;
        AllowNext = _ => true;
    }
        
    public Func<NavigationContext, Task> BeforeAction { get; set; }
    public Func<NavigationContext, Task> AfterAction { get; set; }
    public WizardStepConfiguration LastStep { get; set; }
    public WizardStepConfiguration NextStep { get; set; }
    public Func<NavigationContext, bool> AllowBack { get; set; }
    public Func<NavigationContext, bool> AllowNext { get; set; }
    public InstanceResult Instances { get; set; }
    public bool Initialized { get; set; }
}