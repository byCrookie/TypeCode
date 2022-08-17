using System.Windows.Threading;

namespace TypeCode.Wpf.Helper.Thread;

public static class MainThread
{
    public static void BackgroundFireAndForget(Action action, DispatcherPriority priority)
    {
        System.Windows.Application.Current.Dispatcher.Invoke(action, priority);
    }
}