﻿using System;
using System.Windows.Input;

namespace TypeCode.Wpf.Helper.Commands
{
	public class RelayCommand : ICommand
	{
		private readonly Action<object> _execute;
		private readonly Predicate<object> _canExecute;

		public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
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
