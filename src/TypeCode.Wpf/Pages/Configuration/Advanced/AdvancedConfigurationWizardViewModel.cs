using System.IO;
using System.Xml.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TypeCode.Business.Bootstrapping.Data;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.Navigation.Wizard;
using TypeCode.Wpf.Helper.ViewModels;

namespace TypeCode.Wpf.Pages.Configuration.Advanced;

public partial class AdvancedConfigurationWizardViewModel : ViewModelBase, IAsyncInitialNavigated
{
    private readonly IUserDataLocationProvider _userDataLocationProvider;

    public AdvancedConfigurationWizardViewModel(IUserDataLocationProvider userDataLocationProvider)
    {
        _userDataLocationProvider = userDataLocationProvider;
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
        var cfg = _userDataLocationProvider.GetConfigurationFilePath();
        await File.WriteAllTextAsync(cfg, FormatXml(Configuration)).ConfigureAwait(true);
        await ReloadAsync().ConfigureAwait(true);
    }

    [RelayCommand]
    private async Task ReloadAsync()
    {
        var cfg = _userDataLocationProvider.GetConfigurationFilePath();
        var xml = await File.ReadAllTextAsync(cfg).ConfigureAwait(true);
        Configuration = FormatXml(xml);
    }

    private static string? FormatXml(string? xml)
    {
        if (string.IsNullOrEmpty(xml))
        {
            return xml;
        }

        var doc = XDocument.Parse(xml);
        return doc.ToString();
    }

    [ObservableProperty]
    private string? _configuration;
}