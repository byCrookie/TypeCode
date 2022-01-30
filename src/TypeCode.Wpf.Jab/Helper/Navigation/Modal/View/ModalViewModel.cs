using System.Threading.Tasks;
using System.Windows.Input;
using AsyncAwaitBestPractices.MVVM;
using TypeCode.Wpf.Jab.Helper.Navigation.Contract;
using TypeCode.Wpf.Jab.Helper.Navigation.Modal.Service;
using TypeCode.Wpf.Jab.Helper.Navigation.Service;
using TypeCode.Wpf.Jab.Helper.ViewModel;

namespace TypeCode.Wpf.Jab.Helper.Navigation.Modal.View;

public class ModalViewModel : Reactive, IAsyncNavigatedTo
{
    private readonly IModalNavigationService _modalNavigationService;

    public ModalViewModel(IModalNavigationService modalNavigationService)
    {
        _modalNavigationService = modalNavigationService;
    }
        
    public Task OnNavigatedToAsync(NavigationContext context)
    {
        OkCommand = new AsyncCommand(OkAsync);
        var parameter = context.GetParameter<ModalParameter>();
        Title = parameter.Title;
        Text = parameter.Text;
        return Task.CompletedTask;
    }

    private Task OkAsync()
    {
        return _modalNavigationService.CloseModalAsync();
    }

    public ICommand OkCommand { get; set; }

    public string Title {
        get => Get<string>();
        set => Set(value);
    }

    public string Text {
        get => Get<string>();
        private set => Set(value);
    }
}