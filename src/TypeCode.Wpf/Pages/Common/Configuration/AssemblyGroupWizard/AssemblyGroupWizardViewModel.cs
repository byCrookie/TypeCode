using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;

namespace TypeCode.Wpf.Pages.Common.Configuration.AssemblyGroupWizard;

public partial class AssemblyGroupWizardViewModel : ObservableValidator
{
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    private string? _name;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    private int? _priority;
}