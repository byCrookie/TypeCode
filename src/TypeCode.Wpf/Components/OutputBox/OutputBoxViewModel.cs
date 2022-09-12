using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TypeCode.Business.Format;
using TypeCode.Wpf.Helper.ViewModels;

namespace TypeCode.Wpf.Components.OutputBox;

public sealed partial class OutputBoxViewModel : ViewModelBase
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
    
    [RelayCommand(CanExecute = nameof(ClearOuput))]
    private Task ClearOuputAsync()
    {
        SetOutput(null);
        return Task.CompletedTask;
    }
    
    private bool ClearOuput()
    {
        return !string.IsNullOrEmpty(Output);
    }

    public void SetOutput(string? output)
    {
        Output = output;
        ClearOuputCommand.NotifyCanExecuteChanged();
    }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(CopyToClipboardCommand))]
    private string? _output;
}