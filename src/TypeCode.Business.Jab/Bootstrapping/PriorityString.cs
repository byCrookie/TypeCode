namespace TypeCode.Business.Bootstrapping;

public class PriorityString
{
    public string Priority { get; }
    public string Message { get; }

    public PriorityString(string priority, string message)
    {
        Priority = priority;
        Message = message;
    }
}