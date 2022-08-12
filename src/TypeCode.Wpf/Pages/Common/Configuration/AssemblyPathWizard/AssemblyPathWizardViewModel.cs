using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;

namespace TypeCode.Wpf.Pages.Common.Configuration.AssemblyPathWizard;

public partial class AssemblyPathWizardViewModel : ObservableObject
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
    private string? _path;

    [ObservableProperty]
    private int? _priority;
}