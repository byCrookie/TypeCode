using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using AsyncAwaitBestPractices.MVVM;
using TypeCode.Wpf.Jab.Helper.Navigation.Contract;
using TypeCode.Wpf.Jab.Helper.ViewModel;

namespace TypeCode.Wpf.Jab.Helper.Navigation.Wizard.Complex;

public class WizardViewModel : Reactive, IWizardHost
{
    private readonly IWizardNavigator _wizardNavigator;
    private Wizard _wizard;

    public WizardViewModel(IWizardNavigator wizardNavigator)
    {
        _wizardNavigator = wizardNavigator;
    }

    public async Task NavigateToAsync(Wizard wizard, NavigationAction navigationAction)
    {
        wizard.NavigationContext.AddOrUpdateParameter(navigationAction);
            
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

        WizardPage = wizard.CurrentStepConfiguration.Instances.ViewInstance as UserControl;

        if (wizard.CurrentStepConfiguration.Instances.ViewModelInstance is IAsyncNavigatedTo asyncNavigatedTo)
        {
            await asyncNavigatedTo.OnNavigatedToAsync(wizard.NavigationContext).ConfigureAwait(true);
        }

        _wizard = wizard;
    }

    public async Task NavigateFromAsync(Wizard wizard, NavigationAction navigationAction)
    {
        wizard.NavigationContext.AddOrUpdateParameter(navigationAction);

        if (wizard.CurrentStepConfiguration.Instances.ViewModelInstance is IAsyncNavigatedFrom asyncNavigatedFrom)
        {
            await asyncNavigatedFrom.OnNavigatedFromAsync(wizard.NavigationContext).ConfigureAwait(true);
        }

        await wizard.CurrentStepConfiguration.AfterAction(wizard.NavigationContext).ConfigureAwait(true);
    }

    private Task NextAsync()
    {
        return _wizardNavigator.NextAsync(_wizard);
    }

    private Task BackAsync()
    {
        return _wizardNavigator.BackAsync(_wizard);
    }

    private Task CancelAsync()
    {
        return _wizardNavigator.CancelAsync(_wizard);
    }

    private Task FinishAsync()
    {
        return _wizardNavigator.FinishAsync(_wizard);
    }

    public UserControl WizardPage
    {
        get => Get<UserControl>();
        set => Set(value);
    }

    public ICommand BackCommand
    {
        get => Get<ICommand>();
        set => Set(value);
    }

    public ICommand NextCommand
    {
        get => Get<ICommand>();
        set => Set(value);
    }

    public ICommand CancelCommand
    {
        get => Get<ICommand>();
        set => Set(value);
    }

    public ICommand FinishCommand
    {
        get => Get<ICommand>();
        set => Set(value);
    }
}