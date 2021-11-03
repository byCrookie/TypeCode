using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using AsyncAwaitBestPractices.MVVM;
using TypeCode.Wpf.Helper.Navigation.Contract;
using TypeCode.Wpf.Helper.ViewModel;

namespace TypeCode.Wpf.Helper.Navigation.Wizard.Complex
{
    public class WizardViewModel : Reactive, IWizardHost
    {
        private readonly IWizardNavigator _wizardNavigator;
        private Wizard _wizard;

        public WizardViewModel(IWizardNavigator wizardNavigator)
        {
            _wizardNavigator = wizardNavigator;
        }

        public async Task NavigateToAsync(Wizard wizard)
        {
            await wizard.CurrentStepConfiguration.BeforeAction(wizard.NavigationContext).ConfigureAwait(true);

            BackCommand = new AsyncCommand(Back, _ => wizard.CurrentStepConfiguration != wizard.StepConfigurations.FirstOrDefault()
                                                      && wizard.CurrentStepConfiguration.AllowBack(wizard.NavigationContext));
            NextCommand = new AsyncCommand(Next, _ => wizard.CurrentStepConfiguration != wizard.StepConfigurations.LastOrDefault()
                                                      && wizard.CurrentStepConfiguration.AllowNext(wizard.NavigationContext));
            CancelCommand = new AsyncCommand(Cancel);
            FinishCommand = new AsyncCommand(Finish, _ => wizard.CurrentStepConfiguration == wizard.StepConfigurations.LastOrDefault()
                                                          && wizard.CurrentStepConfiguration.AllowNext(wizard.NavigationContext));

            WizardPage = wizard.CurrentStepConfiguration.Instances.ViewInstance as UserControl;

            if (wizard.CurrentStepConfiguration.Instances.ViewModelInstance is IAsyncNavigatedTo asyncNavigatedTo)
            {
                await asyncNavigatedTo.OnNavigatedToAsync(wizard.NavigationContext).ConfigureAwait(true);
            }

            _wizard = wizard;
        }

        public async Task NavigateFromAsync(Wizard wizard)
        {
            if (wizard.CurrentStepConfiguration.Instances.ViewModelInstance is IAsyncNavigatedFrom asyncNavigatedFrom)
            {
                await asyncNavigatedFrom.OnNavigatedFromAsync(wizard.NavigationContext).ConfigureAwait(true);
            }

            await wizard.CurrentStepConfiguration.AfterAction(wizard.NavigationContext).ConfigureAwait(true);
        }

        private Task Next()
        {
            return _wizardNavigator.Next(_wizard);
        }

        private Task Back()
        {
            return _wizardNavigator.Back(_wizard);
        }

        private Task Cancel()
        {
            return _wizardNavigator.Cancel(_wizard);
        }

        private Task Finish()
        {
            return _wizardNavigator.Finish(_wizard);
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
}