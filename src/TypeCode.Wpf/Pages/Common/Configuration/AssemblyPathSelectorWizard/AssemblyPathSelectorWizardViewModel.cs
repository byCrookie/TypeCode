using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using TypeCode.Wpf.Helper.Validation;
using TypeCode.Wpf.Helper.ViewModels;

namespace TypeCode.Wpf.Pages.Common.Configuration.AssemblyPathSelectorWizard;

public partial class AssemblyPathSelectorWizardViewModel : ViewModelBase
{
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    private string? _selector;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    [CustomValidation(typeof(CustomIntValidation), nameof(CustomIntValidation.ValidateInt))]
    private int? _priority;
    
    [ObservableProperty]
    private bool _ignore;
}