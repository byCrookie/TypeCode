namespace TypeCode.Wpf.Helper.Navigation.Modal.Service;

public sealed class ModalParameter
{
    public ModalParameter()
    {
        Buttons = ModalButtons.Ok;
        OnOkAsync = () => Task.CompletedTask;
        OnCancelAsync = () => Task.CompletedTask;
    }
    
    public string? Title { get; set; }
    public string? Text { get; set; }
    public ModalButtons Buttons { get; set; }
    public Func<Task> OnOkAsync { get; set; }
    public Func<Task> OnCancelAsync { get; set; }
}