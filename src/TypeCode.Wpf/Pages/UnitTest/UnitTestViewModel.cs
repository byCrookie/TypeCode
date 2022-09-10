using CommunityToolkit.Mvvm.Input;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.ViewModels;
using TypeCode.Wpf.Pages.UnitTest.UnitTestDependencyManually;
using TypeCode.Wpf.Pages.UnitTest.UnitTestDependencyType;

namespace TypeCode.Wpf.Pages.UnitTest;

public partial class UnitTestViewModel : ViewModelBase
{
    private readonly INavigationService _navigationService;

    public UnitTestViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }
    
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