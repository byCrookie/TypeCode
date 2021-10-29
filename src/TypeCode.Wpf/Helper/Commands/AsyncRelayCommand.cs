using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TypeCode.Wpf.Helper.Commands
{
	public class AsyncRelayCommand : ICommand
	{
		private readonly Func<object, Task> _execute;
		private readonly Predicate<object> _canExecute;

		public AsyncRelayCommand(Func<object, Task> execute, Predicate<object> canExecute = null)
		{
			_canExecute = canExecute;
			_execute = execute;
		}
		
		public bool CanExecute(object parameter)
		{
			return _canExecute == null || _canExecute(parameter);
		}

		public void Execute(object parameter)
		{
			_execute?.Invoke(parameter);
		}

		public event EventHandler CanExecuteChanged;
	}
}
