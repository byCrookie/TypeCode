using TypeCode.Wpf.Helper.Navigation.Service;

namespace TypeCode.Wpf.Helper.Navigation.Contract;

public interface IAsyncNavigatedFrom
{
    Task OnNavigatedFromAsync(NavigationContext context);
}