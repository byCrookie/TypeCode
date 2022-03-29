namespace TypeCode.Wpf.Components.InputBox;

public class InputBoxViewModelParameter
{
    public InputBoxViewModelParameter(string actionName, Func<bool, string?, Task> actionAsync)
    {
        ActionName = actionName;
        ActionAsync = actionAsync;
    }

    public string? ToolTip { get; set; }
    public string ActionName { get; }
    public Func<bool, string?, Task> ActionAsync { get; }
}