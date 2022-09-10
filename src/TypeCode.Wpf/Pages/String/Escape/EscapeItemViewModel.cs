using TypeCode.Wpf.Helper.ViewModels;

namespace TypeCode.Wpf.Pages.String.Escape;

public class EscapeItemViewModel : ViewModelBase
{
    public string ToEscape { get; }
    public string EscapeWith { get; }

    public EscapeItemViewModel(string toEscape, string escapeWith)
    {
        ToEscape = toEscape;
        EscapeWith = escapeWith;
    }

    public override string ToString()
    {
        return $"- {ToEscape} -> {EscapeWith}";
    }
}