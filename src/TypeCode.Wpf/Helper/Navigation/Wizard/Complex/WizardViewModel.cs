using System.Windows.Controls;
using System.Windows.Input;
using AsyncAwaitBestPractices.MVVM;
using TypeCode.Wpf.Helper.Commands;
using TypeCode.Wpf.Helper.Navigation.Contract;
using TypeCode.Wpf.Helper.ViewModel;

namespace TypeCode.Wpf.Helper.Navigation.Wizard.Complex;

public class WizardViewModel : Reactive, IWizardHost
{
    private readonly IWizardNavigator _wizardNavigator;
    private Wizard? _wizard;

    public WizardViewModel(IWizardNavigator wizardNavigator)
    {
        _wizardNavigator = wizardNavigator;

        BackCommand = new AsyncDefaultCommand();
        NextCommand = new AsyncDefaultCommand();
        CancelCommand = new AsyncDefaultCommand();
        FinishCommand = new AsyncDefaultCommand();
    }

    public async Task NavigateToAsync(Wizard wizard, NavigationAction navigationAction)
    {
        wizard.NavigationContext.AddOrUpdateParameter(navigationAction);

        if (wizard.CurrentStepConfiguration is null)
        {
            throw new ArgumentException($"{nameof(wizard.CurrentStepConfiguration)} is not set");
        }
            
        if (!wizard.CurrentStepConfiguration.Initialized && wizard.CurrentStepConfiguration.Instances.ViewModelInstance is IAsyncInitialNavigated asyncInitialNavigated)
        {
            await asyncInitialNavigated.OnInititalNavigationAsync(wizard.NavigationContext).ConfigureAwait(true);
            wizard.CurrentStepConfiguration.Initialized = true;
        }

        await wizard.CurrentStepConfiguration.BeforeAction(wizard.NavigationContext).ConfigureAwait(true);

        BackCommand = new AsyncCommand(BackAsync, _ => wizard.CurrentStepConfiguration != wizard.StepConfigurations.FirstOrDefault()
                                                       && wizard.CurrentStepConfiguration.AllowBack(wizard.NavigationContext));
        NextCommand = new AsyncCommand(NextAsync, _ => wizard.CurrentStepConfiguration != wizard.StepConfigurations.LastOrDefault()
                                                       && wizard.CurrentStepConfiguration.AllowNext(wizard.NavigationContext));
        CancelCommand = new AsyncCommand(CancelAsync);
        FinishCommand = new AsyncCommand(FinishAsync, _ => wizard.CurrentStepConfiguration == wizard.StepConfigurations.LastOrDefault()
                                                           && wizard.CurrentStepConfiguration.AllowNext(wizard.NavigationContext));

        if (wizard.CurrentStepConfiguration.Instances.ViewInstance is not UserControl wizardPage)
        {
            throw new ArgumentException($"{wizard.CurrentStepConfiguration.Instances.ViewInstance.GetType().FullName} is not a {nameof(UserControl)}");
        }

        WizardPage = wizardPage;

        if (wizard.CurrentStepConfiguration.Instances.ViewModelInstance is IAsyncNavigatedTo asyncNavigatedTo)
        {
            await asyncNavigatedTo.OnNavigatedToAsync(wizard.NavigationContext).ConfigureAwait(true);
        }

        _wizard = wizard;
    }

    public async Task NavigateFromAsync(Wizard wizard, NavigationAction navigationAction)
    {
        wizard.NavigationContext.AddOrUpdateParameter(navigationAction);
        
        if (wizard.CurrentStepConfiguration is null)
        {
            throw new ArgumentException($"{nameof(wizard.CurrentStepConfiguration)} is not set");
        }

        if (wizard.CurrentStepConfiguration.Instances.ViewModelInstance is IAsyncNavigatedFrom asyncNavigatedFrom)
        {
            await asyncNavigatedFrom.OnNavigatedFromAsync(wizard.NavigationContext).ConfigureAwait(true);
        }

        await wizard.CurrentStepConfiguration.AfterAction(wizard.NavigationContext).ConfigureAwait(true);
    }

    private Task NextAsync()
    {
        if (_wizard is null)
        {
            throw new ArgumentException($"Wizard is not set");
        }
        
        return _wizardNavigator.NextAsync(_wizard);
    }

    private Task BackAsync()
    {
        if (_wizard is null)
        {
            throw new ArgumentException($"Wizard is not set");
        }
        
        return _wizardNavigator.BackAsync(_wizard);
    }

    private Task CancelAsync()
    {
        if (_wizard is null)
        {
            throw new ArgumentException($"Wizard is not set");
        }
        
        return _wizardNavigator.CancelAsync(_wizard);
    }

    private Task FinishAsync()
    {
        if (_wizard is null)
        {
            throw new ArgumentException("Wizard is not set");
        }
        
        return _wizardNavigator.FinishAsync(_wizard);
    }

    public UserControl? WizardPage
    {
        get => Get<UserControl?>();
        set => Set(value);
    }

    public ICommand? BackCommand
    {
        get => Get<ICommand?>();
        set => Set(value);
    }

    public ICommand? NextCommand
    {
        get => Get<ICommand?>();
        set => Set(value);
    }

    public ICommand? CancelCommand
    {
        get => Get<ICommand?>();
        set => Set(value);
    }

    public ICommand? FinishCommand
    {
        get => Get<ICommand?>();
        set => Set(value);
    }
}