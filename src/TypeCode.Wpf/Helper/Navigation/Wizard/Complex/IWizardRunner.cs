using System.Threading.Tasks;

namespace TypeCode.Wpf.Helper.Navigation.Wizard.Complex
{
    public interface IWizardRunner
    {
        public Task RunAsync(Wizard wizard);
    }
}