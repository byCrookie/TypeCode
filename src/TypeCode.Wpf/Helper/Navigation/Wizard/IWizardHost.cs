namespace TypeCode.Wpf.Helper.Navigation.Wizard;

public interface IWizardHost
{
    public Task NavigateToAsync(Wizard wizard, NavigationAction navigationAction);
    public Task NavigateFromAsync(Wizard wizard, NavigationAction navigationAction);
}