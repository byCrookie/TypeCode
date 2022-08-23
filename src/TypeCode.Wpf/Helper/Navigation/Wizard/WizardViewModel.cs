using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using AsyncAwaitBestPractices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TypeCode.Wpf.Helper.ViewModels;

namespace TypeCode.Wpf.Helper.Navigation.Wizard;

public partial class WizardViewModel : ObservableObject, IWizardHost
{
    private readonly IWizardNavigator _wizardNavigator;
    private Wizard? _wizard;
    private readonly CancellationTokenSource _cancellationTokenSource;

    public WizardViewModel(IWizardNavigator wizardNavigator)
    {
        _wizardNavigator = wizardNavigator;

        BackCommand = new AsyncRelayCommand(() => Task.CompletedTask);
        NextCommand = new AsyncRelayCommand(() => Task.CompletedTask);
        CancelCommand = new AsyncRelayCommand(() => Task.CompletedTask);
        FinishCommand = new AsyncRelayCommand(() => Task.CompletedTask);

        _cancellationTokenSource = new CancellationTokenSource();
    }

    public async Task NavigateToAsync(Wizard wizard, NavigationAction navigationAction)
    {
        wizard.NavigationContext.AddOrUpdateParameter(navigationAction);

        if (wizard.CurrentStepConfiguration is null)
        {
            throw new ArgumentException($"{nameof(wizard.CurrentStepConfiguration)} is not set");
        }
        
        wizard.NavigationContext.AddOrUpdateParameter(wizard.CurrentStepConfiguration.Instances.ViewModelInstance);

        if (!wizard.CurrentStepConfiguration.Initialized && wizard.CurrentStepConfiguration.Instances.ViewModelInstance is IAsyncInitialNavigated asyncInitialNavigated)
        {
            await asyncInitialNavigated.OnInititalNavigationAsync(wizard.NavigationContext).ConfigureAwait(true);
            wizard.CurrentStepConfiguration.Initialized = true;
        }

        await wizard.CurrentStepConfiguration.BeforeAction(wizard.NavigationContext).ConfigureAwait(true);

        BackCommand = new AsyncRelayCommand(BackAsync, () => wizard.CurrentStepConfiguration != wizard.StepConfigurations.FirstOrDefault()
                                                             && wizard.CurrentStepConfiguration.AllowBack(wizard.NavigationContext));
        IsBackButtonVisible = wizard.CurrentStepConfiguration != wizard.StepConfigurations.FirstOrDefault();
        
        NextCommand = new AsyncRelayCommand(NextAsync, () => wizard.CurrentStepConfiguration != wizard.StepConfigurations.LastOrDefault()
                                                             && wizard.CurrentStepConfiguration.AllowNext(wizard.NavigationContext));
        IsNextButtonVisible = wizard.CurrentStepConfiguration != wizard.StepConfigurations.LastOrDefault();
        
        CancelCommand = new AsyncRelayCommand(CancelAsync);
        
        FinishCommand = new AsyncRelayCommand(FinishAsync, () => wizard.CurrentStepConfiguration == wizard.StepConfigurations.LastOrDefault()
                                                                 && wizard.CurrentStepConfiguration.AllowNext(wizard.NavigationContext));
        IsFinishButtonVisible = wizard.CurrentStepConfiguration == wizard.StepConfigurations.LastOrDefault();
        FinishText = wizard.FinishText;

        if (wizard.CurrentStepConfiguration.Instances.ViewInstance is not UserControl wizardPage)
        {
            throw new ArgumentException($"{wizard.CurrentStepConfiguration.Instances.ViewInstance.GetType().FullName} is not a {nameof(UserControl)}");
        }
        
        UpdateWizardAsync().SafeFireAndForget();

        WizardPage = wizardPage;
        
        await NavigationCaller.CallNavigateToAsync(wizard.CurrentStepConfiguration.Instances.ViewModelInstance, wizard.NavigationContext).ConfigureAwait(true);

        _wizard = wizard;
    }

    public async Task NavigateFromAsync(Wizard wizard, NavigationAction navigationAction)
    {
        _cancellationTokenSource.TryReset();
        
        wizard.NavigationContext.AddOrUpdateParameter(navigationAction);

        if (wizard.CurrentStepConfiguration is null)
        {
            throw new ArgumentException($"{nameof(wizard.CurrentStepConfiguration)} is not set");
        }

        await NavigationCaller.CallNavigateFromAsync(wizard.CurrentStepConfiguration.Instances.ViewModelInstance, wizard.NavigationContext).ConfigureAwait(true);

        await wizard.CurrentStepConfiguration.AfterAction(wizard.NavigationContext).ConfigureAwait(true);
    }
    
    private async Task UpdateWizardAsync()
    {
        while (!_cancellationTokenSource.IsCancellationRequested)
        {
            BackCommand?.NotifyCanExecuteChanged();
            NextCommand?.NotifyCanExecuteChanged();
            CancelCommand?.NotifyCanExecuteChanged();
            FinishCommand?.NotifyCanExecuteChanged();
            await Task.Delay(10).ConfigureAwait(true);
        }
    }

    private Task NextAsync()
    {
        if (_wizard is null)
        {
            throw new ArgumentException("Wizard is not set");
        }

        return _wizardNavigator.NextAsync(_wizard);
    }

    private Task BackAsync()
    {
        if (_wizard is null)
        {
            throw new ArgumentException("Wizard is not set");
        }

        return _wizardNavigator.BackAsync(_wizard);
    }

    private Task CancelAsync()
    {
        _cancellationTokenSource.Cancel();
        
        if (_wizard is null)
        {
            throw new ArgumentException("Wizard is not set");
        }

        return _wizardNavigator.CancelAsync(_wizard);
    }

    private Task FinishAsync()
    {
        _cancellationTokenSource.Cancel();
        
        if (_wizard is null)
        {
            throw new ArgumentException("Wizard is not set");
        }

        return _wizardNavigator.FinishAsync(_wizard);
    }

    [ObservableProperty]
    private UserControl? _wizardPage;

    [ObservableProperty]
    private AsyncRelayCommand? _backCommand;

    [ObservableProperty]
    private AsyncRelayCommand? _nextCommand;

    [ObservableProperty]
    private AsyncRelayCommand? _cancelCommand;

    [ObservableProperty]
    private AsyncRelayCommand? _finishCommand;

    [ObservableProperty]
    private bool _isBackButtonVisible;

    [ObservableProperty]
    private bool _isNextButtonVisible;

    [ObservableProperty]
    private bool _isFinishButtonVisible;

    [ObservableProperty]
    private string? _finishText;
}