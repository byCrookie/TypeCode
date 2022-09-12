using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TypeCode.Wpf.Components.NavigationCard;
using TypeCode.Wpf.Helper.Navigation.Contract;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.ViewModels;
using TypeCode.Wpf.Pages.UnitTest.UnitTestDependencyManually;
using TypeCode.Wpf.Pages.UnitTest.UnitTestDependencyType;

namespace TypeCode.Wpf.Pages.UnitTest;

public sealed partial class UnitTestViewModel : ViewModelBase, IAsyncNavigatedTo
{
    private readonly INavigationService _navigationService;
    private readonly INavigationCardViewModelFactory _navigationCardViewModelFactory;

    public UnitTestViewModel(
        INavigationService navigationService,
        INavigationCardViewModelFactory navigationCardViewModelFactory
    )
    {
        _navigationService = navigationService;
        _navigationCardViewModelFactory = navigationCardViewModelFactory;
    }

    public Task OnNavigatedToAsync(NavigationContext context)
    {
        TypeCardViewModel = _navigationCardViewModelFactory.Create(new NavigationCardViewModelParameter(
            "Type",
            "Search for a type in your loaded assemblies and let TypeCode generate your unit-test code."
            , NavigateToTypeCommand)
        );

        ManuallyCardViewModel = _navigationCardViewModelFactory.Create(new NavigationCardViewModelParameter(
            "Manually",
            "Paste the code of your constructor directly, let TypeCode parse it and generate your unit-test code."
            , NavigateToManuallyCommand)
        );

        return Task.CompletedTask;
    }

    [ObservableProperty]
    [ChildViewModel]
    private NavigationCardViewModel? _manuallyCardViewModel;

    [ObservableProperty]
    [ChildViewModel]
    private NavigationCardViewModel? _typeCardViewModel;

    [RelayCommand]
    private Task NavigateToManuallyAsync()
    {
        return _navigationService.NavigateAsync<UnitTestDependencyManuallyViewModel>(new NavigationContext());
    }

    [RelayCommand]
    private Task NavigateToTypeAsync()
    {
        return _navigationService.NavigateAsync<UnitTestDependencyTypeViewModel>(new NavigationContext());
    }
}