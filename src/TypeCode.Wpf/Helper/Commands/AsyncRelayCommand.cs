using AsyncAwaitBestPractices.MVVM;

namespace TypeCode.Wpf.Helper.Commands;

public class AsyncRelayCommand : AsyncCommand
{
    public AsyncRelayCommand(Func<Task> execute, Func<object?, bool> canExecute)
        : base(execute, canExecute, e =>
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() => new Exception(e.Message,e));
        }, true)
    {
    }

    public AsyncRelayCommand(Func<Task> execute)
        : base(execute, null, e =>
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() => new Exception(e.Message,e));
        }, true)
    {
    }
}