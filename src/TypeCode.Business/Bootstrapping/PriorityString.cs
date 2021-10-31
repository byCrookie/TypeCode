namespace TypeCode.Business.Bootstrapping
{
    public class PriorityString
    {
        public int Priority { get; }
        public string Message { get; }

        public PriorityString(int priority, string message)
        {
            Priority = priority;
            Message = message;
        }
    }
}