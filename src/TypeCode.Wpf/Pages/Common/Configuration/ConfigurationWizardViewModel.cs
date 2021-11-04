using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Controls;
using TypeCode.Business.Configuration;
using TypeCode.Wpf.Helper.Navigation.Contract;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.ViewModel;

namespace TypeCode.Wpf.Pages.Common.Configuration
{
    public class ConfigurationWizardViewModel : Reactive, IAsyncNavigated
    {
        private readonly IConfigurationMapper _configurationMapper;

        public ConfigurationWizardViewModel(IConfigurationMapper configurationMapper)
        {
            _configurationMapper = configurationMapper;
            
            TypeCodeConfigurations = new ObservableCollection<TypeCodeConfiguration>();
        }
        
        public async Task OnNavigatedToAsync(NavigationContext context)
        {
            var config = await ReadConfigurationAsync().ConfigureAwait(true);
            TypeCodeConfigurations.Add(config);
        }

        public Task OnNavigatedFromAsync(NavigationContext context)
        {
            return Task.CompletedTask;
        }

        public ObservableCollection<TypeCodeConfiguration> TypeCodeConfigurations {
            get => Get<ObservableCollection<TypeCodeConfiguration>>();
            set => Set(value);
        }
        
        private async Task<TypeCodeConfiguration> ReadConfigurationAsync()
        {
            var cfg = $@"{Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)}\Configuration.cfg.xml";
            var xml = await File.ReadAllTextAsync(cfg).ConfigureAwait(true);
            return _configurationMapper.MapToConfiguration(GenericXmlSerializer.Deserialize<XmlTypeCodeConfiguration>(xml));
        }
    }
}