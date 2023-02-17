using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TypeCode.Wpf.Helper.Navigation.Contract;
using TypeCode.Wpf.Helper.Navigation.Modal.Service;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.ViewModels;

namespace TypeCode.Wpf.Helper.Navigation.Modal.View;

public sealed partial class ModalViewModel : ViewModelBase, IAsyncNavigatedTo
{
    private readonly IModalNavigationService _modalNavigationService;

    public ModalViewModel(IModalNavigationService modalNavigationService)
    {
        _modalNavigationService = modalNavigationService;
    }
        
    public Task OnNavigatedToAsync(NavigationContext context)
    {
        var parameter = context.GetParameter<ModalParameter>();
        Title = parameter.Title;
        Text = parameter.Text;
        OkVisible = parameter.Buttons is ModalButtons.Ok or ModalButtons.OkAndCancel;
        OkText = parameter.OkText;
        CancelVisible = parameter.Buttons is ModalButtons.OkAndCancel;
        CancelText = parameter.CancelText;
        return Task.CompletedTask;
    }

    [RelayCommand]
    private Task OkAsync()
    {
        return _modalNavigationService.OkAsync();
    }
    
    [RelayCommand]
    private Task CancelAsync()
    {
        return _modalNavigationService.CancelAsync();
    }

    [ObservableProperty]
    private string? _title;

    [ObservableProperty]
    private string? _text;
    
    [ObservableProperty]
    private string? _okText;
    
    [ObservableProperty]
    private bool _okVisible;
    
    [ObservableProperty]
    private string? _cancelText;
    
    [ObservableProperty]
    private bool _cancelVisible;
}