using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using TypeCode.Wpf.Helper.ViewModels;

namespace TypeCode.Wpf.Pages.Common.Configuration.IncludeAssemblyPatternWizard;

public partial class IncludeAssemblyPatternWizardViewModel : ViewModelBase
{
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    private string? _pattern;
}