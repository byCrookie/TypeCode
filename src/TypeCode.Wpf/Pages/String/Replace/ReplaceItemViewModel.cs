using TypeCode.Wpf.Helper.ViewModels;

namespace TypeCode.Wpf.Pages.String.Replace;

public sealed class ReplaceItemViewModel : ViewModelBase
{
    public string ToReplace { get; }
    public string ReplaceWith { get; }

    public ReplaceItemViewModel(string toReplace, string replaceWith)
    {
        ToReplace = toReplace;
        ReplaceWith = replaceWith;
    }

    public override string ToString()
    {
        return $"{ToReplace} -> {ReplaceWith}";
    }
}