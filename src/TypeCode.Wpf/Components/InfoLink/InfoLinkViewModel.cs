using CommunityToolkit.Mvvm.ComponentModel;
using TypeCode.Wpf.Helper.ViewModels;

namespace TypeCode.Wpf.Components.InfoLink;

public partial class InfoLinkViewModel : ViewModelBase
{
    [ObservableProperty]
    private string? _link;
}