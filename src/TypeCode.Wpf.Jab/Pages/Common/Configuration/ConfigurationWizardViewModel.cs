using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using AsyncAwaitBestPractices.MVVM;
using TypeCode.Wpf.Jab.Helper.Navigation.Service;
using TypeCode.Wpf.Jab.Helper.Navigation.Wizard.Complex;
using TypeCode.Wpf.Jab.Helper.ViewModel;

namespace TypeCode.Wpf.Jab.Pages.Common.Configuration;

public class ConfigurationWizardViewModel : Reactive, IAsyncInitialNavigated
{
    public Task OnInititalNavigationAsync(NavigationContext context)
    {
        ReloadCommand = new AsyncCommand(ReloadAsync);
        SaveCommand = new AsyncCommand(SaveAsync);
        FormatCommand = new AsyncCommand(FormatAsync);
            
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
        
    private static string FormatXml(string xml)
    {
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

    public string Configuration
    {
        get => Get<string>();
        set => Set(value);
    }
}