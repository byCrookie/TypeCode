﻿using System.Windows.Controls;
using System.Windows.Input;
using AsyncAwaitBestPractices.MVVM;
using TypeCode.Wpf.Helper.Navigation.Contract;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.ViewModel;

namespace TypeCode.Wpf.Helper.Navigation.Wizard.WizardSimple;

public class WizardSimpleViewModel<T> : Reactive, IAsyncNavigatedTo
{
    private readonly IWizardNavigationService _wizardNavigationService;

    public WizardSimpleViewModel(IWizardNavigationService wizardNavigationService)
    {
        _wizardNavigationService = wizardNavigationService;
        
        FinishCommand = new AsyncCommand(FinishAsync);
    }
        
    public Task OnNavigatedToAsync(NavigationContext context)
    {
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