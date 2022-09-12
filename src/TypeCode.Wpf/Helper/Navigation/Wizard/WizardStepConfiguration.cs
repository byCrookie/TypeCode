using TypeCode.Wpf.Helper.Navigation.Service;

namespace TypeCode.Wpf.Helper.Navigation.Wizard;

public sealed class WizardStepConfiguration
{
    public WizardStepConfiguration(
        Func<NavigationContext, Task> afterAction,
        Func<NavigationContext, Task> beforeAction,
        Func<NavigationContext, bool> allowBack,
        Func<NavigationContext, bool> allowNext,
        InstanceResult instances,
        string? stepTitle
    )
    {
        AfterAction = afterAction;
        BeforeAction = beforeAction;
        AllowBack = allowBack;
        AllowNext = allowNext;
        Instances = instances;
        StepTitle = stepTitle;
    }

    public Func<NavigationContext, Task> BeforeAction { get; }
    public Func<NavigationContext, Task> AfterAction { get; }
    public Func<NavigationContext, bool> AllowBack { get; }
    public Func<NavigationContext, bool> AllowNext { get; }
    public InstanceResult Instances { get; }

    public WizardStepConfiguration? LastStep { get; set; }
    public WizardStepConfiguration? NextStep { get; set; }
    public string? StepTitle { get; }
}