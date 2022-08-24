using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;

namespace TypeCode.Wpf.Pages.Common.Configuration.IncludeAssemblyPatternWizard;

public partial class IncludeAssemblyPatternWizardViewModel : ObservableValidator
{
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    private string? _pattern;
}