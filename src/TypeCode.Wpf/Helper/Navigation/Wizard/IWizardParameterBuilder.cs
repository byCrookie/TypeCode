using TypeCode.Wpf.Helper.Navigation.Service;

namespace TypeCode.Wpf.Helper.Navigation.Wizard;

public interface IWizardParameterBuilder
{
    WizardParameterBuilder Before(Func<NavigationContext, Task> action);
    WizardParameterBuilder After(Func<NavigationContext, Task> action);
    WizardParameterBuilder AllowBack(Func<NavigationContext, bool> allow);
    WizardParameterBuilder AllowNext(Func<NavigationContext, bool> allow);
    WizardParameterBuilder Title(string title);
    WizardStepParameter Build();
}