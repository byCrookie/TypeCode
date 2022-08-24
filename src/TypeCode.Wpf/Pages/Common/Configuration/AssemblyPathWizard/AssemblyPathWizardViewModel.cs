using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;

namespace TypeCode.Wpf.Pages.Common.Configuration.AssemblyPathWizard;

public partial class AssemblyPathWizardViewModel : ObservableValidator
{
    [RelayCommand]
    private Task SelectAsync()
    {
        var openFileDialog = new OpenFileDialog
        {
            Multiselect = false,
            Title = "Select Assembly Path",
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
    private int? _priority;
}