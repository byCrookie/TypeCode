using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TypeCode.Wpf.Helper.Navigation.Contract;
using TypeCode.Wpf.Helper.Navigation.Modal.Service;
using TypeCode.Wpf.Helper.Navigation.Service;

namespace TypeCode.Wpf.Helper.Navigation.Modal.View;

public partial class ModalViewModel : ObservableObject, IAsyncNavigatedTo
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
        ScrollViewerEnabled = parameter.ScrollViewerDisabled;
        return Task.CompletedTask;
    }

    [RelayCommand]
    private Task OkAsync()
    {
        return _modalNavigationService.CloseModalAsync();
    }

    [ObservableProperty]
    private string? _title;

    [ObservableProperty]
    private string? _text;

    [ObservableProperty]
    private bool _scrollViewerEnabled;
}