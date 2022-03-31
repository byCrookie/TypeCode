using TypeCode.Business.Version;
using TypeCode.Wpf.Helper.Navigation.Contract;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.ViewModel;

namespace TypeCode.Wpf.Pages.Home;

public class HomeViewModel : Reactive, IAsyncNavigatedTo
{
    private readonly IVersionEvaluator _versionEvaluator;

    public HomeViewModel(IVersionEvaluator versionEvaluator)
    {
        _versionEvaluator = versionEvaluator;
    }

    public string? Version
    {
        get => Get<string?>();
        set => Set(value);
    }

    public async Task OnNavigatedToAsync(NavigationContext context)
    {
        var version = await _versionEvaluator.ReadCurrentVersionAsync().ConfigureAwait(false);
        Version = version.CurrentVersion;
    }
}