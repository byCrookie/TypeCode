using System.Windows.Input;
using AsyncAwaitBestPractices.MVVM;
using Microsoft.Win32;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.Navigation.Wizard.Complex;
using TypeCode.Wpf.Helper.ViewModel;

namespace TypeCode.Wpf.Pages.Common.Configuration.AssemblyRoot;

public class AssemblyRootWizardViewModel : Reactive, IAsyncInitialNavigated
{
    public AssemblyRootWizardViewModel()
    {
        SelectCommand = new AsyncCommand(SelectAsync);
    }

    public Task OnInititalNavigationAsync(NavigationContext context)
    {
        return Task.CompletedTask;
    }

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

    public ICommand SelectCommand { get; set; }

    public string? Path
    {
        get => Get<string?>();
        set => Set(value);
    }

    public int? Priority
    {
        get => Get<int?>();
        set => Set(value);
    }
}