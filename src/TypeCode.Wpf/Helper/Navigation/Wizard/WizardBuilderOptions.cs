using System.Windows;
using System.Windows.Controls;
using TypeCode.Wpf.Helper.Navigation.Service;

namespace TypeCode.Wpf.Helper.Navigation.Wizard;

public class WizardBuilderOptions
{
    public WizardBuilderOptions(NavigationContext context, Frame navigationFrame, UIElement content, UIElement wizardOverlay)
    {
        Context = context;
        NavigationFrame = navigationFrame;
        Content = content;
        WizardOverlay = wizardOverlay;
    }

    public NavigationContext Context { get; }
    public Frame NavigationFrame { get; }
    public UIElement Content { get; }
    public UIElement WizardOverlay { get; }
}