using System.Windows;
using System.Windows.Controls;
using TypeCode.Wpf.Helper.Navigation.Service;

namespace TypeCode.Wpf.Helper.Navigation.Wizard.Complex;

public class Wizard
{
    public Wizard(
        WizardBuilderOptions wizardBuilderOptions,
        InstanceResult createInstances
    )
    {
        StepConfigurations = new List<WizardStepConfiguration>();
        CompletedAction = _ => Task.CompletedTask;

        NavigationFrame = wizardBuilderOptions.NavigationFrame;
        Content = wizardBuilderOptions.Content;
        WizardOverlay = wizardBuilderOptions.WizardOverlay;
        NavigationContext = wizardBuilderOptions.Context;
        WizardInstances = createInstances;
    }

    public Frame NavigationFrame { get; }
    public UIElement Content { get; }
    public UIElement WizardOverlay { get; }
    public NavigationContext NavigationContext { get; }
    public InstanceResult WizardInstances { get; }

    public List<WizardStepConfiguration> StepConfigurations { get; }
    public WizardStepConfiguration? CurrentStepConfiguration { get; set; }
    public Func<NavigationContext, Task> CompletedAction { get; set; }
    public object? CompletedEvent { get; set; }
    public string FinishText { get; set; }
}