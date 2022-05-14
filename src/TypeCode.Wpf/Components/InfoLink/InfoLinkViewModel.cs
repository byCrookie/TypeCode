using TypeCode.Wpf.Helper.ViewModel;

namespace TypeCode.Wpf.Components.InfoLink;

public class InfoLinkViewModel : Reactive
{
    public string? Link
    {
        get => Get<string?>();
        set => Set(value);
    }
}