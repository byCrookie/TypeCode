using AsyncAwaitBestPractices.MVVM;

namespace TypeCode.Wpf.Helper.Commands;

public class AsyncRelayCommand : AsyncCommand
{
    public AsyncRelayCommand(Func<Task> execute, Func<object?, bool> canExecute)
        : base(execute, canExecute, e => { System.Windows.Application.Current.Dispatcher.Invoke(() => new Exception(e.Message, e)); })
    {
    }

    public AsyncRelayCommand(Func<Task> execute)
        : base(execute, null, e => { System.Windows.Application.Current.Dispatcher.Invoke(() => new Exception(e.Message, e)); })
    {
    }
}

public class AsyncRelayCommand<T> : AsyncCommand<T>
{
    public AsyncRelayCommand(Func<T?, Task> execute, Func<object?, bool> canExecute)
        : base(execute, canExecute, e => { System.Windows.Application.Current.Dispatcher.Invoke(() => new Exception(e.Message, e)); })
    {
    }

    public AsyncRelayCommand(Func<T?, Task> execute)
        : base(execute, null, e => { System.Windows.Application.Current.Dispatcher.Invoke(() => new Exception(e.Message, e)); })
    {
    }
}