using System.Windows.Threading;
using Serilog;
using TypeCode.Wpf.Helper.Thread;

namespace TypeCode.Wpf.Helper.Event;

public class EventAggregator : IEventAggregator
{
    private readonly object _lock = new();

    private static readonly IDictionary<Type, HashSet<object>> Events = new Dictionary<Type, HashSet<object>>();
    private static readonly IDictionary<object, HashSet<Type>> Subscribers = new Dictionary<object, HashSet<Type>>();

    public void Subscribe<TEvent>(object subscriber) where TEvent : notnull
    {
        lock (_lock)
        {
            if (!Events.ContainsKey(typeof(TEvent)))
            {
                Events.Add(typeof(TEvent), new HashSet<object> { subscriber });
            }
            else
            {
                Events[typeof(TEvent)].Add(subscriber);
            }

            if (!Subscribers.ContainsKey(subscriber))
            {
                Subscribers.Add(subscriber, new HashSet<Type> { typeof(TEvent) });
            }
            else
            {
                Subscribers[subscriber].Add(typeof(TEvent));
            }
        }
    }

    public void Unsubscribe<TEvent>(object subscriber) where TEvent : notnull
    {
        lock (_lock)
        {
            if (Events.ContainsKey(typeof(TEvent)))
            {
                Events[typeof(TEvent)].Remove(subscriber);
            }

            if (Subscribers.ContainsKey(subscriber))
            {
                Subscribers[subscriber].Remove(typeof(TEvent));
            }
        }
    }

    public void Unsubscribe(object subscriber)
    {
        lock (_lock)
        {
            if (Subscribers.ContainsKey(subscriber))
            {
                Subscribers.Remove(subscriber, out var events);

                if (events is null)
                {
                    return;
                }

                foreach (var @event in events.Where(@event => Events.ContainsKey(@event)))
                {
                    Events[@event].Remove(subscriber);
                }
            }
        }
    }

    public Task PublishAsync<TEvent>(TEvent e) where TEvent : notnull
    {
        Log.Debug("{Event} was published", typeof(TEvent));

        lock (_lock)
        {
            if (Events.ContainsKey(typeof(TEvent)))
            {
                foreach (var subscriber in Events[typeof(TEvent)])
                {
                    if (subscriber is IAsyncEventHandler<TEvent> subsriberHandler)
                    {
                        Log.Debug("Calling {Handler} to handle {Event}", subsriberHandler.GetType().FullName, typeof(TEvent));
                        MainThread.BackgroundFireAndForget(() => subsriberHandler.HandleAsync(e), DispatcherPriority.Send);
                    }
                }
            }
        }

        return Task.CompletedTask;
    }
}