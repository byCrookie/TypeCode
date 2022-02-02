namespace TypeCode.Wpf.Helper.Navigation.Wizard;

public interface IWizardNavigator
{
    Task NextOrNewAsync<TViewModel>(WizardContext context) where TViewModel : notnull;
    Task BackAsync(WizardContext context);
    Task CloseAsync(WizardContext context);
    Task CloseCurrentAsync(WizardContext context);
}