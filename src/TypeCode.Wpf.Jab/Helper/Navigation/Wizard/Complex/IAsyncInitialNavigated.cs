using System.Threading.Tasks;
using TypeCode.Wpf.Jab.Helper.Navigation.Service;

namespace TypeCode.Wpf.Jab.Helper.Navigation.Wizard.Complex;

public interface IAsyncInitialNavigated
{
    Task OnInititalNavigationAsync(NavigationContext context);
}