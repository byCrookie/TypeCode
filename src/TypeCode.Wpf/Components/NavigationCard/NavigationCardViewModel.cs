using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using TypeCode.Wpf.Helper.ViewModels;

namespace TypeCode.Wpf.Components.NavigationCard;

public partial class NavigationCardViewModel : ViewModelBase
{
    [ObservableProperty]
    private string? _title;
    
    [ObservableProperty]
    private string? _description;
    
    [ObservableProperty]
    private ICommand? _navigateCommand;
}