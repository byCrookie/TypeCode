using System.Threading.Tasks;

namespace TypeCode.Wpf.Jab.Helper.Navigation.Wizard.Complex;

public interface IWizardHost
{
    public Task NavigateToAsync(Wizard wizard, NavigationAction navigationAction);
    public Task NavigateFromAsync(Wizard wizard, NavigationAction navigationAction);
}