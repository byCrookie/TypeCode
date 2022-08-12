using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TypeCode.Business.Format;

namespace TypeCode.Wpf.Components.OutputBox;

public partial class OutputBoxViewModel : ObservableObject
{
    [RelayCommand(CanExecute = nameof(CanCopyToClipboard))]
    private Task CopyToClipboardAsync()
    {
        var relevantLines = Output?
            .Split(Environment.NewLine)
            .Where(line => !line.StartsWith(Cuts.Point()))
            .ToList() ?? new List<string>();

        var relevantOutput = string.Join(Environment.NewLine, relevantLines);

        Clipboard.SetText(relevantOutput);
        return Task.CompletedTask;
    }
    
    private bool CanCopyToClipboard()
    {
        return !string.IsNullOrEmpty(Output?.Trim());
    }

    public void SetOutput(string? output)
    {
        Output = output;
    }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(CopyToClipboardCommand))]
    private string? _output;
}