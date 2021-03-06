using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.Navigation.Wizard.Complex;
using TypeCode.Wpf.Helper.ViewModel;

namespace TypeCode.Wpf.Pages.Common.Configuration.AssemblyGroupWizard;

public class AssemblyGroupWizardViewModel : Reactive, IAsyncInitialNavigated
{
    public Task OnInititalNavigationAsync(NavigationContext context)
    {
        return Task.CompletedTask;
    }

    public string? Name
    {
        get => Get<string?>();
        set => Set(value);
    }

    public int? Priority
    {
        get => Get<int?>();
        set => Set(value);
    }
}