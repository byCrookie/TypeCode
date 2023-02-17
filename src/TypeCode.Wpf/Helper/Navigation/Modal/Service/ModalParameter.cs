namespace TypeCode.Wpf.Helper.Navigation.Modal.Service;

public sealed class ModalParameter
{
    public ModalParameter()
    {
        Buttons = ModalButtons.Ok;
        OnOkAsync = () => Task.CompletedTask;
        OnCancelAsync = () => Task.CompletedTask;
        OkText = "Ok";
        CancelText = "Cancel";
    }
    
    public string? Title { get; set; }
    public string? Text { get; set; }
    public ModalButtons Buttons { get; set; }
    public Func<Task> OnOkAsync { get; set; }
    public string OkText { get; set; }
    public string CancelText { get; set; }
    public Func<Task> OnCancelAsync { get; set; }
}