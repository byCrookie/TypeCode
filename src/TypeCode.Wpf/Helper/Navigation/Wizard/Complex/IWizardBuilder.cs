using TypeCode.Wpf.Helper.Navigation.Service;

namespace TypeCode.Wpf.Helper.Navigation.Wizard.Complex;

public interface IWizardBuilder
{
    IWizardBuilder Then<TViewModel>(Action<IWizardParameterBuilder, NavigationContext>? configureParameter = null) where TViewModel : notnull;
    IWizardBuilder FinishAsync(Func<NavigationContext, Task> completedAction);
    IWizardBuilder PublishAsync<TEvent>();
    Wizard Build();
}