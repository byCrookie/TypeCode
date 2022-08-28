using System.IO;
using System.Xml.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TypeCode.Business.Configuration.Location;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.Navigation.Wizard;
using TypeCode.Wpf.Helper.ViewModels;

namespace TypeCode.Wpf.Pages.Common.Configuration;

public partial class ConfigurationWizardViewModel : ViewModelBase, IAsyncInitialNavigated
{
    private readonly IConfigurationLocationProvider _configurationLocationProvider;

    public ConfigurationWizardViewModel(IConfigurationLocationProvider configurationLocationProvider)
    {
        _configurationLocationProvider = configurationLocationProvider;
    }
    
    public Task OnInititalNavigationAsync(NavigationContext context)
    {
        return ReloadAsync();
    }

    [RelayCommand]
    private Task FormatAsync()
    {
        Configuration = FormatXml(Configuration);
        return Task.CompletedTask;
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        var cfg = _configurationLocationProvider.GetLocation();
        await File.WriteAllTextAsync(cfg, FormatXml(Configuration)).ConfigureAwait(true);
        await ReloadAsync().ConfigureAwait(true);
    }

    [RelayCommand]
    private async Task ReloadAsync()
    {
        var cfg = _configurationLocationProvider.GetLocation();
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

    [ObservableProperty]
    private string? _configuration;
}