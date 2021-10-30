using System.Threading.Tasks;

namespace TypeCode.Wpf.Helper.Navigation
{
    public interface IAsyncNavigatedFrom
    {
        Task OnNavigatedFromAsync(NavigationContext context);
    }
}