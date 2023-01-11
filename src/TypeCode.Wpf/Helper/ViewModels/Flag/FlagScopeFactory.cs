namespace TypeCode.Wpf.Helper.ViewModels.Flag;

internal class FlagScopeFactory : IFlagScopeFactory
{
    public AsyncFlagScope CreateAsync(Flag flag)
    {
        return new AsyncFlagScope(flag, _ => Task.CompletedTask);
    }

    public AsyncFlagScope CreateAsync(Flag flag, Action<Flag> onChange)
    {
        return new AsyncFlagScope(flag, localFlag => { 
            onChange(localFlag);
            return Task.CompletedTask;
        });
    }

    public AsyncFlagScope CreateAsync(Flag flag, Func<Flag, Task> onChange)
    {
        return new AsyncFlagScope(flag, onChange);
    }
}