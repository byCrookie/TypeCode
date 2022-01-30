using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using AsyncAwaitBestPractices.MVVM;
using TypeCode.Wpf.Jab.Helper.Navigation.Contract;
using TypeCode.Wpf.Jab.Helper.Navigation.Service;
using TypeCode.Wpf.Jab.Helper.ViewModel;

namespace TypeCode.Wpf.Jab.Helper.Navigation.Wizard.WizardSimple;

public class WizardSimpleViewModel<T> : Reactive, IAsyncNavigatedTo
{
    private readonly IWizardNavigationService _wizardNavigationService;

    public WizardSimpleViewModel(IWizardNavigationService wizardNavigationService)
    {
        _wizardNavigationService = wizardNavigationService;
    }
        
    public Task OnNavigatedToAsync(NavigationContext context)
    {
        FinishCommand = new AsyncCommand(FinishAsync);
        var parameter = context.GetParameter<WizardParameter<T>>();
        FinishText = parameter.FinishButtonText ?? "Close";
        WizardPage = context.GetParameter<UserControl>("View");
        return Task.CompletedTask;
    }

    private Task FinishAsync()
    {
        return _wizardNavigationService.CloseWizardAsync<T>();
    }

    public ICommand FinishCommand { get; set; }

    public UserControl WizardPage {
        get => Get<UserControl>();
        set => Set(value);
    }

    public string FinishText {
        get => Get<string>();
        set => Set(value);
    }
}