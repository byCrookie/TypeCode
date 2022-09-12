using System.Windows.Input;

namespace TypeCode.Wpf.Components.NavigationCard;

public sealed class NavigationCardViewModelParameter
{
    public NavigationCardViewModelParameter(string title, string description, ICommand navigateCommand)
    {
        Title = title;
        Description = description;
        NavigateCommand = navigateCommand;
    }
    
    public string Title { get; }
    public string Description { get; }
    public ICommand NavigateCommand { get; }
}