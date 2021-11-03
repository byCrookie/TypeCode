using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TypeCode.Wpf.Helper.Navigation.Service;

namespace TypeCode.Wpf.Helper.Navigation.Wizard.Complex
{
    public interface IWizardBuilder
    {
        IWizardBuilder Init(NavigationContext context, Frame navigationFrame, UIElement content, UIElement wizardOverlay);
        IWizardBuilder Then<TViewModel>(Action<WizardParameterBuilder, NavigationContext> configureParameter = null);
        IWizardBuilder Finish(Func<NavigationContext, Task> completedAction);
        Wizard Build();
    }
}