namespace TypeCode.Wpf.Helper.Thread;

public interface IMinDelay
{
    Task DelayAsync(DateTime start, TimeSpan minDelay);
}