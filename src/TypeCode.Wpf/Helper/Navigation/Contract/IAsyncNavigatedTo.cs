using System.Threading.Tasks;
using TypeCode.Wpf.Helper.Navigation.Service;

namespace TypeCode.Wpf.Helper.Navigation.Contract
{
    public interface IAsyncNavigatedTo
    {
        Task OnNavigatedToAsync(NavigationContext context);
    }
}