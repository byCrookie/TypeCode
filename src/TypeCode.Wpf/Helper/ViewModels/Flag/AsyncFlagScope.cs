using AsyncAwaitBestPractices;

namespace TypeCode.Wpf.Helper.ViewModels.Flag;

public class AsyncFlagScope : IAsyncDisposable
{
    private readonly Flag _flag;
    private readonly Func<Flag, Task> _onChange;
    private readonly bool _initialState;

    public AsyncFlagScope(Flag flag, Func<Flag, Task> onChange)
    {
        _flag = flag;
        _onChange = onChange;
        _initialState = _flag;
        _flag.Set(!_flag);
        _onChange(_flag).SafeFireAndForget();
    }

    public async ValueTask DisposeAsync()
    {
        _flag.Set(_initialState);
        await _onChange(_flag);
    }
}