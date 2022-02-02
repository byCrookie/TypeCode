using System.Windows.Controls;

namespace TypeCode.Wpf.Helper.Navigation.Wizard;

public class WizardNavigatorParameter
{
    public WizardNavigatorParameter(
        Frame navigationFrame,
        Grid mainContentControl,
        Grid wizardOverlayControl
    )
    {
        NavigationFrame = navigationFrame;
        MainContentControl = mainContentControl;
        WizardOverlayControl = wizardOverlayControl;
    }

    public Frame NavigationFrame { get; }
    public Grid MainContentControl { get; }
    public Grid WizardOverlayControl { get; }
}