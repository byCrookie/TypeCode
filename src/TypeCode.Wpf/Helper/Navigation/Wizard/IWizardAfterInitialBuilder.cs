using TypeCode.Wpf.Helper.Navigation.Service;

namespace TypeCode.Wpf.Helper.Navigation.Wizard;

public interface IWizardAfterInitialBuilder : IWizardBuilder
{
    IWizardAfterInitialBuilder NavigationContext(Action<NavigationContext> modify);
    IWizardAfterInitialBuilder FinishAsync(Func<NavigationContext, Task> completedAction, string? finishText = null);
    IWizardAfterInitialBuilder PublishAsync<TEvent>();
    Wizard Build();
}