using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TypeCode.Wpf.Components.NavigationCard;
using TypeCode.Wpf.Helper.Navigation.Contract;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.ViewModels;
using TypeCode.Wpf.Pages.String.Escape;
using TypeCode.Wpf.Pages.String.Length;

namespace TypeCode.Wpf.Pages.String;

public partial class StringViewModel : ViewModelBase, IAsyncNavigatedTo
{
    private readonly INavigationService _navigationService;
    private readonly INavigationCardViewModelFactory _navigationCardViewModelFactory;

    public StringViewModel(
        INavigationService navigationService,
        INavigationCardViewModelFactory navigationCardViewModelFactory
    )
    {
        _navigationService = navigationService;
        _navigationCardViewModelFactory = navigationCardViewModelFactory;
    }

    public Task OnNavigatedToAsync(NavigationContext context)
    {
        StringLengthViewModel = _navigationCardViewModelFactory.Create(new NavigationCardViewModelParameter(
            "String Length",
            "Simple text length measuring."
            , NavigateToStringLengthCommand)
        );

        StringEscapeViewModel = _navigationCardViewModelFactory.Create(new NavigationCardViewModelParameter(
            "String Escape",
            "Escape text using custom escaping instructions."
            , NavigateToStringEscapeCommand)
        );

        return Task.CompletedTask;
    }

    [ObservableProperty]
    [ChildViewModel]
    private NavigationCardViewModel? _stringLengthViewModel;

    [ObservableProperty]
    [ChildViewModel]
    private NavigationCardViewModel? _stringEscapeViewModel;

    [RelayCommand]
    private Task NavigateToStringLengthAsync()
    {
        return _navigationService.NavigateAsync<StringLengthViewModel>(new NavigationContext());
    }

    [RelayCommand]
    private Task NavigateToStringEscapeAsync()
    {
        return _navigationService.NavigateAsync<StringEscapeViewModel>(new NavigationContext());
    }
}