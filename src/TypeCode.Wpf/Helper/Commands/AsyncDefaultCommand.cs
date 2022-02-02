using AsyncAwaitBestPractices.MVVM;

namespace TypeCode.Wpf.Helper.Commands;

public class AsyncDefaultCommand : AsyncCommand
{
    public AsyncDefaultCommand() 
        : base(() => Task.CompletedTask)
    {
    }
}