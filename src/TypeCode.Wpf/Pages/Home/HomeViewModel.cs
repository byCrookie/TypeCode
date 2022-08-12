using CommunityToolkit.Mvvm.ComponentModel;
using TypeCode.Business.Version;
using TypeCode.Wpf.Helper.Navigation.Contract;
using TypeCode.Wpf.Helper.Navigation.Service;

namespace TypeCode.Wpf.Pages.Home;

public partial class HomeViewModel : ObservableObject, IAsyncNavigatedTo
{
    private readonly IVersionEvaluator _versionEvaluator;

    public HomeViewModel(IVersionEvaluator versionEvaluator)
    {
        _versionEvaluator = versionEvaluator;
    }

    [ObservableProperty]
    private string? _version;

    public async Task OnNavigatedToAsync(NavigationContext context)
    {
        var version = await _versionEvaluator.ReadCurrentVersionAsync().ConfigureAwait(false);
        Version = version.CurrentVersion;
    }
}