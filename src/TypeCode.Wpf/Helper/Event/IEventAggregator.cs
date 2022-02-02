namespace TypeCode.Wpf.Helper.Event;

public interface IEventAggregator
{
    public void Subscribe<TEvent>(object subscriber) where TEvent : notnull;
    public void Unsubscribe<TEvent>(object subscriber) where TEvent : notnull;
    public Task PublishAsync<TEvent>(TEvent e) where TEvent : notnull;
}