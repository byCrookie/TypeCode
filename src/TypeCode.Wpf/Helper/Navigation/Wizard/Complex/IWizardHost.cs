using System.Threading.Tasks;
using TypeCode.Wpf.Helper.Navigation.Service;

namespace TypeCode.Wpf.Helper.Navigation.Wizard.Complex
{
    public interface IWizardHost
    {
        public Task NavigateToAsync(Wizard wizard);
        public Task NavigateFromAsync(Wizard wizard);
    }
}