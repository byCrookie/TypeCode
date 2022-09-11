using TypeCode.Wpf.Helper.Navigation.Service;

namespace TypeCode.Wpf.Helper.Navigation.Contract;

public interface IAsyncInitialNavigated
{
    Task OnInititalNavigationAsync(NavigationContext context);
}