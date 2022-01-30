using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TypeCode.Wpf.Jab.Helper.Navigation.Service;

namespace TypeCode.Wpf.Jab.Helper.Navigation.Wizard.Complex;

public interface IWizardBuilder
{
    IWizardBuilder Init(NavigationContext context, Frame navigationFrame, UIElement content, UIElement wizardOverlay);
    IWizardBuilder Then<TViewModel>(Action<WizardParameterBuilder, NavigationContext> configureParameter = null);
    IWizardBuilder FinishAsync(Func<NavigationContext, Task> completedAction);
    IWizardBuilder PublishAsync<TEvent>();
    Wizard Build();
}