using TypeCode.Wpf.Helper.Navigation.Service;

namespace TypeCode.Wpf.Helper.Navigation.Wizard;

public interface IAsyncInitialNavigated
{
    Task OnInititalNavigationAsync(NavigationContext context);
}