using System.Threading.Tasks;

namespace TypeCode.Wpf.Helper.ViewModel
{
	public class ViewModelBase : Reactive
	{
		public Task OnNavigateToAsync(object parameter)
		{
			return Task.CompletedTask;
		}
	}
}
