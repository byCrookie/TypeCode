using AsyncAwaitBestPractices.MVVM;

namespace TypeCode.Wpf.Helper.Commands;

public class AsyncDefaultCommand : IAsyncCommand
{
    public bool CanExecute(object? parameter)
    {
        return false;
    }

    public void Execute(object? parameter)
    {
    }

    public event EventHandler? CanExecuteChanged;
    
    public Task ExecuteAsync()
    {
        return Task.CompletedTask;
    }

    public void RaiseCanExecuteChanged()
    {
    }
}