using CommunityToolkit.Mvvm.ComponentModel;

namespace TypeCode.Wpf.Pages.Common.Configuration.AssemblyPathSelectorWizard;

public partial class AssemblyPathSelectorWizardViewModel : ObservableObject
{
    [ObservableProperty]
    private string? _selector;

    [ObservableProperty]
    private int? _priority;
}