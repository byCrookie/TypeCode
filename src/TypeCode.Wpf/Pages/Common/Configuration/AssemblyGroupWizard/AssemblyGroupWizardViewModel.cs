using CommunityToolkit.Mvvm.ComponentModel;

namespace TypeCode.Wpf.Pages.Common.Configuration.AssemblyGroupWizard;

public partial class AssemblyGroupWizardViewModel : ObservableObject
{
    [ObservableProperty]
    private string? _name;

    [ObservableProperty]
    private int? _priority;
}