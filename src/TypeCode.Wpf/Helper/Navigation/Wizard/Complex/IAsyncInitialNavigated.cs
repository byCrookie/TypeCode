using System.Threading.Tasks;
using TypeCode.Wpf.Helper.Navigation.Service;

namespace TypeCode.Wpf.Helper.Navigation.Wizard.Complex
{
    public interface IAsyncInitialNavigated
    {
        Task OnInititalNavigationAsync(NavigationContext context);
    }
}