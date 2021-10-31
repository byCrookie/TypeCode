using System.Threading.Tasks;
using System.Windows.Input;
using AsyncAwaitBestPractices.MVVM;
using TypeCode.Wpf.Helper.Navigation;
using TypeCode.Wpf.Helper.ViewModel;

namespace TypeCode.Wpf.Helper.Modal
{
    public class ModalViewModel : Reactive, IAsyncNavigatedTo
    {
        private readonly INavigationService _navigationService;

        public ModalViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }
        
        public Task OnNavigatedToAsync(NavigationContext context)
        {
            OkCommand = new AsyncCommand(Ok);
            var parameter = context.GetParameter<ModalParameter>();
            Title = parameter.Title;
            Text = parameter.Text;
            return Task.CompletedTask;
        }

        private Task Ok()
        {
            return _navigationService.CloseModal();
        }

        public ICommand OkCommand { get; set; }

        public string Title {
            get => Get<string>();
            set => Set(value);
        }

        public string Text {
            get => Get<string>();
            private set => Set(value);
        }
    }
}