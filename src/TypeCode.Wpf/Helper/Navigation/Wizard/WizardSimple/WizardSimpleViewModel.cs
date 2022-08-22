﻿using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TypeCode.Wpf.Helper.Event;
using TypeCode.Wpf.Helper.Navigation.Contract;
using TypeCode.Wpf.Helper.Navigation.Service;

namespace TypeCode.Wpf.Helper.Navigation.Wizard.WizardSimple;

public partial class WizardSimpleViewModel<T> :
    ObservableValidator,
    IAsyncEventHandler<WizardUpdateEvent>,
    IAsyncNavigatedTo,
    IAsyncNavigatedFrom where T : notnull
{
    private readonly IWizardNavigationService _wizardNavigationService;
    private readonly IEventAggregator _eventAggregator;
    private WizardParameter<T> _parameter;
    private NavigationContext _context;

    public WizardSimpleViewModel(IWizardNavigationService wizardNavigationService, IEventAggregator eventAggregator)
    {
        _wizardNavigationService = wizardNavigationService;
        _eventAggregator = eventAggregator;

        eventAggregator.Subscribe<WizardUpdateEvent>(this);

        _parameter = new WizardParameter<T>();
        _context = new NavigationContext();
    }

    public Task OnNavigatedToAsync(NavigationContext context)
    {
        _context = context;
        _parameter = _context.GetParameter<WizardParameter<T>>();
        FinishText = _parameter.FinishButtonText ?? "Close";
        WizardPage = _context.GetParameter<UserControl>("View");
        return Task.CompletedTask;
    }

    public Task OnNavigatedFromAsync(NavigationContext context)
    {
        _eventAggregator.Unsubscribe(this);
        return Task.CompletedTask;
    }

    [RelayCommand]
    private Task CancelAsync()
    {
        return _wizardNavigationService.CloseWizardAsync<T>();
    }

    [RelayCommand(CanExecute = nameof(CanFinish))]
    private Task FinishAsync()
    {
        return _wizardNavigationService.SaveWizardAsync<T>();
    }

    private bool CanFinish()
    {
        var viewModel = _context.GetParameter<T>("ViewModel");
        return viewModel is not ObservableValidator { HasErrors: true } && _parameter.CanSave(viewModel);
    }

    [ObservableProperty]
    private UserControl? _wizardPage;

    [ObservableProperty]
    private string? _finishText;

    public Task HandleAsync(WizardUpdateEvent e, CancellationToken? cancellationToken = null)
    {
        FinishCommand.NotifyCanExecuteChanged();
        return Task.CompletedTask;
    }
}