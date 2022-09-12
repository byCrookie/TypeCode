namespace TypeCode.Wpf.Components.NavigationCard;

public sealed class NavigationCardViewModelFactory : INavigationCardViewModelFactory
{
    public NavigationCardViewModel Create(NavigationCardViewModelParameter parameter)
    {
        return new NavigationCardViewModel
        {
            Title = parameter.Title,
            Description = parameter.Description,
            NavigateCommand = parameter.NavigateCommand
        };
    }
}