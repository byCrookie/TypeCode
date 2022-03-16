using System.Windows.Controls;
using System.Windows.Input;
using AsyncAwaitBestPractices.MVVM;
using TypeCode.Wpf.Helper.Event;
using TypeCode.Wpf.Helper.Navigation.Contract;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.ViewModel;

namespace TypeCode.Wpf.Helper.Navigation.Wizard.WizardSimple;

public class WizardSimpleViewModel<T> : 
    Reactive,
    IAsyncEventHandler<WizardUpdateEvent>,
    IAsyncNavigatedTo where T : notnull
{
    private readonly IWizardNavigationService _wizardNavigationService;
    private WizardParameter<T> _parameter;
    private NavigationContext _context;

    public WizardSimpleViewModel(IWizardNavigationService wizardNavigationService, IEventAggregator eventAggregator)
    {
        _wizardNavigationService = wizardNavigationService;
        
        eventAggregator.Subscribe<WizardUpdateEvent>(this);

        _parameter = new WizardParameter<T>();
        _context = new NavigationContext();
        
        CancelCommand = new AsyncCommand(CancelAsync);
        FinishCommand = new AsyncCommand(FinishAsync, CanFinish);
    }

    public Task OnNavigatedToAsync(NavigationContext context)
    {
        _context = context;
        _parameter = _context.GetParameter<WizardParameter<T>>();
        FinishText = _parameter.FinishButtonText ?? "Close";
        WizardPage = _context.GetParameter<UserControl>("View");
        return Task.CompletedTask;
    }

    private Task CancelAsync()
    {
        return _wizardNavigationService.CloseWizardAsync<T>();
    }
    
    private bool CanFinish(object? arg)
    {
        return _parameter.CanSave(_context.GetParameter<T>("ViewModel"));
    }

    private Task FinishAsync()
    {
        return _wizardNavigationService.SaveWizardAsync<T>();
    }

    public IAsyncCommand FinishCommand { get; set; }
    public ICommand CancelCommand { get; set; }

    public UserControl? WizardPage {
        get => Get<UserControl?>();
        set => Set(value);
    }

    public string? FinishText {
        get => Get<string?>();
        set => Set(value);
    }

    public Task HandleAsync(WizardUpdateEvent e)
    {
        FinishCommand.RaiseCanExecuteChanged();
        return Task.CompletedTask;
    }
}