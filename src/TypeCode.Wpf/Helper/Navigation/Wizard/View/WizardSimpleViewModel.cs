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
            
            BackCommand = new AsyncCommand(Back);
            NextCommand = new AsyncCommand(Next);
            CancelCommand = new AsyncCommand(Cancel);
            FinishCommand = new AsyncCommand(Finish);
            
            WizardPage = context.GetParameter<UserControl>("View");
            return Task.CompletedTask;
        }
        
        private Task Next()
        {
            return _wizardNavigator.CloseCurrentAsync(_context.GetParameter<WizardContext>("WizardContext"));
        }
        
        private Task Back()
        {
            return _wizardNavigator.BackAsync(_context.GetParameter<WizardContext>("WizardContext"));
        }
        
        private Task Cancel()
        {
            return _wizardNavigator.CloseAsync(_context.GetParameter<WizardContext>("WizardContext"));
        }

        private Task Finish()
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