namespace TypeCode.Wpf.Helper.ViewModels.Flag;

public class Flag
{
    private bool _state;
    
    public Flag(bool state = false)
    {
        Set(state);
    }

    public void Set(bool state = false)
    {
        _state = state;
    }

    public static implicit operator bool(Flag foo)
    {
        return foo is { _state: true };
    }
}