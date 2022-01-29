using System.Threading.Tasks;

namespace TypeCode.Wpf.Helper.Navigation.Wizard.Complex;

public interface IWizardNavigator
{
    Task NextAsync(Wizard wizard);
    Task BackAsync(Wizard wizard);
    Task CancelAsync(Wizard wizard);
    Task FinishAsync(Wizard wizard);
}