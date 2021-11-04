using System.Threading.Tasks;

namespace TypeCode.Wpf.Helper.Navigation.Wizard.Complex
{
    public interface IWizardNavigator
    {
        Task Next(Wizard wizard);
        Task Back(Wizard wizard);
        Task Cancel(Wizard wizard);
        Task Finish(Wizard wizard);
    }
}