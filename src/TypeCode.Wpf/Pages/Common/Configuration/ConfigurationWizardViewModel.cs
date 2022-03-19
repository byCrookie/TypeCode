using System.IO;
using System.Windows.Input;
using System.Xml.Linq;
using TypeCode.Wpf.Helper.Commands;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.Navigation.Wizard.Complex;
using TypeCode.Wpf.Helper.ViewModel;

namespace TypeCode.Wpf.Pages.Common.Configuration;

public class ConfigurationWizardViewModel : Reactive, IAsyncInitialNavigated
{
    public ConfigurationWizardViewModel()
    {
        ReloadCommand = new AsyncRelayCommand(ReloadAsync);
        SaveCommand = new AsyncRelayCommand(SaveAsync);
        FormatCommand = new AsyncRelayCommand(FormatAsync);
    }
    
    public Task OnInititalNavigationAsync(NavigationContext context)
    {
        return ReloadAsync();
    }

    private Task FormatAsync()
    {
        Configuration = FormatXml(Configuration);
        return Task.CompletedTask;
    }

    private async Task SaveAsync()
    {
        var cfg = $@"{Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)}\Configuration.cfg.xml";
        await File.WriteAllTextAsync(cfg, FormatXml(Configuration)).ConfigureAwait(true);
        await ReloadAsync().ConfigureAwait(true);
    }

    private async Task ReloadAsync()
    {
        var cfg = $@"{Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)}\Configuration.cfg.xml";
        var xml = await File.ReadAllTextAsync(cfg).ConfigureAwait(true);
        Configuration = FormatXml(xml);
    }
        
    private static string? FormatXml(string? xml)
    {
        if (string.IsNullOrEmpty(xml))
        {
            return xml;
        }
        
        try
        {
            var doc = XDocument.Parse(xml);
            return doc.ToString();
        }
        catch (Exception)
        {
            return xml;
        }
    }

    public ICommand ReloadCommand { get; set; }
    public ICommand SaveCommand { get; set; }
    public ICommand FormatCommand { get; set; }

    public string? Configuration
    {
        get => Get<string?>();
        set => Set(value);
    }
}