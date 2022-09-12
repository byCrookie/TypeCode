using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using TypeCode.Wpf.Helper.Validation;
using TypeCode.Wpf.Helper.ViewModels;

namespace TypeCode.Wpf.Pages.Configuration.Simple.AssemblyRootWizard;

public sealed partial class AssemblyRootWizardViewModel : ViewModelBase
{
    [RelayCommand]
    private Task SelectAsync()
    {
        var openFileDialog = new OpenFileDialog
        {
            Multiselect = false,
            Title = "Select Assembly Root",
            ValidateNames = false,
            CheckFileExists = false,
            CheckPathExists = true,
            FileName = "Selected Folder"
        };

        if (openFileDialog.ShowDialog() == true)
        {
            Path = System.IO.Path.GetDirectoryName(openFileDialog.FileName);
        }

        return Task.CompletedTask;
    }

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    [DataType(DataType.Url)]
    private string? _path;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    [CustomValidation(typeof(CustomIntValidation), nameof(CustomIntValidation.ValidateInt))]
    private string? _priority;
    
    [ObservableProperty]
    private bool _ignore;
}