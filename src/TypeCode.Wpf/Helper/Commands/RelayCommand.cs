using System;
using System.Windows.Input;

namespace TypeCode.Wpf.Helper.Commands
{
	class RelayCommand : ICommand
	{
		private readonly Action<object> _execute;
		private readonly Predicate<object> _canExecute;

		public RelayCommand(Action<object> execute)
			: this(null, execute) { }
		
		public RelayCommand(Predicate<object> canExecute, Action<object> execute)
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
