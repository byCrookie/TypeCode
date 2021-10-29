using System.Windows.Input;
using TypeCode.Wpf.Helper;

namespace TypeCode.Wpf
{
	public class MainViewModel : ViewModelBase
	{
		private string _testProperty;

		public MainViewModel()
		{
			TestCommand = new RelayCommand(TestMethod);
		}

		public ICommand TestCommand { get; set; }

		private void TestMethod(object parameter)
		{

		}

		public string TestProperty
		{
			get { return _testProperty; }
			set
			{
				if (value == _testProperty) return;
				_testProperty = value;
				OnPropertyChanged();
			}
		}
	}
}