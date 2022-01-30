using System.Threading.Tasks;
using TypeCode.Wpf.Jab.Helper.Navigation.Service;

namespace TypeCode.Wpf.Jab.Helper.Navigation.Contract;

public interface IAsyncNavigatedTo
{
    Task OnNavigatedToAsync(NavigationContext context);
}