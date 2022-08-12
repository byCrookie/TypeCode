using CommunityToolkit.Mvvm.ComponentModel;

namespace TypeCode.Wpf.Pages.Common.Configuration.IncludeAssemblyPatternWizard;

public partial class IncludeAssemblyPatternWizardViewModel : ObservableObject
{
    [ObservableProperty]
    private string? _pattern;
}