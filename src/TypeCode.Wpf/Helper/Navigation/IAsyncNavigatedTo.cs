using System.Threading.Tasks;

namespace TypeCode.Wpf.Helper.Navigation
{
    public interface IAsyncNavigatedTo
    {
        Task OnNavigatedToAsync(NavigationContext context);
    }
}