using System.Windows;
using AsyncAwaitBestPractices.MVVM;
using TypeCode.Wpf.Helper.Commands;
using TypeCode.Wpf.Helper.ViewModel;

namespace TypeCode.Wpf.Components.OutputBox;

public class OutputBoxViewModel : Reactive
{
    public OutputBoxViewModel()
    {
        CopyToClipboardCommand = new AsyncRelayCommand(() =>
        {
            Clipboard.SetText(Output ?? string.Empty);
            return Task.CompletedTask;
        }, _ => !string.IsNullOrEmpty(Output?.Trim()));
    }
    
    public void SetOutput(string? output)
    {
        Output = output;
    }
    
    public IAsyncCommand CopyToClipboardCommand { get; set; }

    public string? Output
    {
        get => Get<string?>();
        set
        {
            Set(value);
            CopyToClipboardCommand.RaiseCanExecuteChanged();
        }
    }
}