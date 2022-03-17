using System.Windows.Input;
using AsyncAwaitBestPractices.MVVM;
using TypeCode.Wpf.Helper.Navigation.Contract;
using TypeCode.Wpf.Helper.Navigation.Modal.Service;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.ViewModel;

namespace TypeCode.Wpf.Helper.Navigation.Modal.View;

public class ModalViewModel : Reactive, IAsyncNavigatedTo
{
    private readonly IModalNavigationService _modalNavigationService;

    public ModalViewModel(IModalNavigationService modalNavigationService)
    {
        _modalNavigationService = modalNavigationService;
        
        OkCommand = new AsyncCommand(OkAsync);
    }
        
    public Task OnNavigatedToAsync(NavigationContext context)
    {
        var parameter = context.GetParameter<ModalParameter>();
        Title = parameter.Title;
        Text = parameter.Text;
        ScrollViewerEnabled = parameter.ScrollViewerDisabled;
        return Task.CompletedTask;
    }

    private Task OkAsync()
    {
        return _modalNavigationService.CloseModalAsync();
    }

    public ICommand OkCommand { get; set; }

    public string? Title {
        get => Get<string?>();
        set => Set(value);
    }

    public string? Text {
        get => Get<string?>();
        private set => Set(value);
    }
    
    public bool ScrollViewerEnabled {
        get => Get<bool>();
        private set => Set(value);
    }
}