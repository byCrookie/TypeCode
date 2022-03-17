namespace TypeCode.Wpf.Helper.Navigation.Modal.Service;

public class ModalParameter
{
    public ModalParameter()
    {
        OnCloseAsync = () => Task.CompletedTask;
    }
    
    public string? Title { get; set; }
    public string? Text { get; set; }
    public bool ScrollViewerDisabled { get; set; }
    public Func<Task> OnCloseAsync { get; set; }
}