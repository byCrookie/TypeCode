using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using AsyncAwaitBestPractices.MVVM;
using TypeCode.Wpf.Helper.Navigation.Contract;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.ViewModel;

namespace TypeCode.Wpf.Helper.Navigation.Wizard.View
{
    public class WizardSimpleViewModel : Reactive, IAsyncNavigatedTo
    {
        private readonly IWizardNavigator _wizardNavigator;
        private NavigationContext _context;

        public WizardSimpleViewModel(IWizardNavigator wizardNavigator)
        {
            _wizardNavigator = wizardNavigator;
        }
        
        public Task OnNavigatedToAsync(NavigationContext context)
        {
            _context = context;
            
            BackCommand = new AsyncCommand(BackAsync);
            NextCommand = new AsyncCommand(NextAsync);
            CancelCommand = new AsyncCommand(CancelAsync);
            FinishCommand = new AsyncCommand(FinishAsync);
            
            WizardPage = context.GetParameter<UserControl>("View");
            return Task.CompletedTask;
        }
        
        private Task NextAsync()
        {
            return _wizardNavigator.CloseCurrentAsync(_context.GetParameter<WizardContext>("WizardContext"));
        }
        
        private Task BackAsync()
        {
            return _wizardNavigator.BackAsync(_context.GetParameter<WizardContext>("WizardContext"));
        }
        
        private Task CancelAsync()
        {
            return _wizardNavigator.CloseAsync(_context.GetParameter<WizardContext>("WizardContext"));
        }

        private Task FinishAsync()
        {
            return _wizardNavigator.CloseAsync(_context.GetParameter<WizardContext>("WizardContext"));
        }

        public ICommand BackCommand { get; set; }
        public ICommand NextCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand FinishCommand { get; set; }

        public UserControl WizardPage {
            get => Get<UserControl>();
            set => Set(value);
        }
    }
}