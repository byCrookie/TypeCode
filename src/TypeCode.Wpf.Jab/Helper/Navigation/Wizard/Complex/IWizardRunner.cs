using System.Threading.Tasks;

namespace TypeCode.Wpf.Jab.Helper.Navigation.Wizard.Complex;

public interface IWizardRunner
{
    public Task RunAsync(Wizard wizard);
}