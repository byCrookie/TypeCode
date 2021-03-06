using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace TypeCode.Wpf.Helper.ViewModel;

public class Reactive : INotifyPropertyChanged
{
    private readonly Dictionary<string, object?> _properties = new();

    protected T? Get<T>([CallerMemberName] string? name = null)
    {
        Debug.Assert(name != null, "name != null");
        if (_properties.TryGetValue(name, out var value))
        {
            return value is null ? default : (T)value;
        }

        return default;
    }

    protected void Set<T>(T value, [CallerMemberName] string? name = null)
    {
        Debug.Assert(name != null, "name != null");
        if (!Equals(value, Get<T>(name)))
        {
            _properties[name] = value;
            OnPropertyChanged(name);
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}