namespace TypeCode.Wpf.Components.NavigationCard;

public interface INavigationCardViewModelFactory
{
    NavigationCardViewModel Create(NavigationCardViewModelParameter parameter);
}