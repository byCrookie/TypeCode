namespace TypeCode.Wpf.Helper.ViewModels.Flag;

public interface IFlagScopeFactory
{
    AsyncFlagScope CreateAsync(Flag flag);
    AsyncFlagScope CreateAsync(Flag flag, Action<Flag> onChange);
    AsyncFlagScope CreateAsync(Flag flag, Func<Flag, Task> onChange);
}