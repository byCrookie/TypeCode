using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using AsyncAwaitBestPractices.MVVM;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.Navigation.Wizard.Complex;
using TypeCode.Wpf.Helper.ViewModel;

namespace TypeCode.Wpf.Pages.Common.Configuration
{
    public class ConfigurationWizardViewModel : Reactive, IAsyncInitialNavigated
    {
        public async Task OnInititalNavigationAsync(NavigationContext context)
        {
            ReloadCommand = new AsyncCommand(ReloadAsync);
            SaveCommand = new AsyncCommand(SaveAsync);
            
            await ReloadAsync().ConfigureAwait(true);
        }

        private Task SaveAsync()
        {
            var cfg = $@"{Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)}\Configuration.cfg.xml";
            return File.WriteAllTextAsync(cfg, Configuration);
        }

        private async Task ReloadAsync()
        {
            var cfg = $@"{Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)}\Configuration.cfg.xml";
            Configuration = await File.ReadAllTextAsync(cfg).ConfigureAwait(true);
        }

        public ICommand ReloadCommand { get; set; }
        public ICommand SaveCommand { get; set; }

        public string Configuration
        {
            get => Get<string>();
            set => Set(value);
        }
    }
}