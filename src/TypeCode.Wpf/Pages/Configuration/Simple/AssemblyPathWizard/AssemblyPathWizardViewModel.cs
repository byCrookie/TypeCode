﻿using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using TypeCode.Wpf.Helper.Validation;
using TypeCode.Wpf.Helper.ViewModels;

namespace TypeCode.Wpf.Pages.Configuration.Simple.AssemblyPathWizard;

public sealed partial class AssemblyPathWizardViewModel : ViewModelBase
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
    [CustomValidation(typeof(CustomIntValidation), nameof(CustomIntValidation.ValidateInt))]
    private int? _priority;
    
    [ObservableProperty]
    private bool _ignore;
}