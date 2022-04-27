using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.Navigation.Wizard.Complex;
using TypeCode.Wpf.Helper.ViewModel;

namespace TypeCode.Wpf.Pages.Common.Configuration.IncludeAssemblyPatternWizard;

public class IncludeAssemblyPatternWizardViewModel : Reactive, IAsyncInitialNavigated
{
    public Task OnInititalNavigationAsync(NavigationContext context)
    {
        return Task.CompletedTask;
    }

    public string? Pattern
    {
        get => Get<string?>();
        set => Set(value);
    }
}