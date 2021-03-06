using TypeCode.Wpf.Helper.Navigation.Service;

namespace TypeCode.Wpf.Helper.Navigation.Wizard.Complex;

public interface IWizardAfterInitialBuilder : IWizardBuilder
{
    IWizardAfterInitialBuilder FinishAsync(Func<NavigationContext, Task> completedAction, string? finishText = null);
    IWizardAfterInitialBuilder PublishAsync<TEvent>();
    Wizard Build();
}