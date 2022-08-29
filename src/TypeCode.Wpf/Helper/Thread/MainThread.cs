using System.Windows.Threading;

namespace TypeCode.Wpf.Helper.Thread;

public static class MainThread
{
    public static void BackgroundFireAndForgetSync(Action action, DispatcherPriority priority)
    {
        System.Windows.Application.Current.Dispatcher.Invoke(action, priority);
    }
    
    public static void BackgroundFireAndForgetSync(Func<Task> action, DispatcherPriority priority)
    {
        System.Windows.Application.Current.Dispatcher.Invoke(action, priority);
    }
    
    public static void BackgroundFireAndForgetAsync(Action action, DispatcherPriority priority)
    {
        System.Windows.Application.Current.Dispatcher.InvokeAsync(action, priority);
    }
    
    public static void BackgroundFireAndForgetAsync(Func<Task> action, DispatcherPriority priority)
    {
        System.Windows.Application.Current.Dispatcher.InvokeAsync(action, priority);
    }
}