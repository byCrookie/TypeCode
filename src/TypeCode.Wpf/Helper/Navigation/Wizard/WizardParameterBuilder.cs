using TypeCode.Wpf.Helper.Navigation.Service;

namespace TypeCode.Wpf.Helper.Navigation.Wizard;

public class WizardParameterBuilder : IWizardParameterBuilder
{
    private readonly WizardStepParameter _parameter;

    public WizardParameterBuilder()
    {
        _parameter = new WizardStepParameter();
    }

    public WizardParameterBuilder Before(Func<NavigationContext, Task> action)
    {
        _parameter.BeforeAction = action;
        return this;
    }

    public WizardParameterBuilder After(Func<NavigationContext, Task> action)
    {
        _parameter.AfterAction = action;
        return this;
    }

    public WizardParameterBuilder AllowBack(Func<NavigationContext, bool> allow)
    {
        _parameter.AllowBack = allow;
        return this;
    }

    public WizardParameterBuilder AllowNext(Func<NavigationContext, bool> allow)
    {
        _parameter.AllowNext = allow;
        return this;
    }

    public WizardParameterBuilder Title(string title)
    {
        _parameter.StepTitle = title;
        return this;
    }

    public WizardStepParameter Build()
    {
        return _parameter;
    }
}