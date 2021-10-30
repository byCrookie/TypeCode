using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace TypeCode.Wpf.Helper.ViewModel
{
	public class ViewModelBase : INotifyPropertyChanged
	{
		public Task OnNavigateToAsync(object parameter)
		{
			return Task.CompletedTask;
		}
		
		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
