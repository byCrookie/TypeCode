namespace TypeCode.Business.Bootstrapping
{
    internal class PriorityString
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