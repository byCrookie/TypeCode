using Framework.Time;

namespace TypeCode.Wpf.Helper.Thread;

public class MinDelay : IMinDelay
{
    private readonly IDateTimeProvider _dateTimeProvider;

    public MinDelay(IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
    }
    
    public async Task DelayAsync(DateTime start, TimeSpan minDelay)
    {
        var diff = _dateTimeProvider.Now() - start;

        if (diff < minDelay)
        {
            await Task.Delay(minDelay - diff).ConfigureAwait(true);
        }
    }
}