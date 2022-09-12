namespace TypeCode.Wpf.Components.InputBox;

public sealed class InputBoxViewModelParameter
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