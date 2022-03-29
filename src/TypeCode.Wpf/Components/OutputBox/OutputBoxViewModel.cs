using System.Windows;
using AsyncAwaitBestPractices.MVVM;
using TypeCode.Business.Format;
using TypeCode.Wpf.Helper.Commands;
using TypeCode.Wpf.Helper.ViewModel;

namespace TypeCode.Wpf.Components.OutputBox;

public class OutputBoxViewModel : Reactive
{
    public OutputBoxViewModel()
    {
        CopyToClipboardCommand = new AsyncRelayCommand(() =>
        {
            var relevantLines = Output?
                .Split(Environment.NewLine)
                .Where(line => !line.StartsWith(Cuts.Point()))
                .ToList() ?? new List<string>();
            
            var relevantOutput = string.Join(Environment.NewLine, relevantLines);
            
            Clipboard.SetText(relevantOutput);
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