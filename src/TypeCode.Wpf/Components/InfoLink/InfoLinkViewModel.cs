using CommunityToolkit.Mvvm.ComponentModel;

namespace TypeCode.Wpf.Components.InfoLink;

public partial class InfoLinkViewModel : ObservableObject
{
    [ObservableProperty]
    private string? _link;
}