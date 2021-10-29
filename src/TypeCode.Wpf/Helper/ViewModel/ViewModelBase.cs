using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TypeCode.Wpf.Helper.ViewModel
{
	public class ViewModelBase : INotifyPropertyChanged
	{
		public virtual void OnNavigateTo(object parameter)
		{
		}
		
		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
