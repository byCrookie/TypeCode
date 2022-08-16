using System.Collections.Concurrent;
using System.Windows.Threading;
using Framework.Extensions.List;
using Serilog;
using TypeCode.Wpf.Helper.Thread;

namespace TypeCode.Wpf.Helper.Event;

public class EventAggregator : IEventAggregator
{
    private static readonly ConcurrentDictionary<Type, ConcurrentDictionary<object, object?>> Events = new();
    private static readonly ConcurrentDictionary<object, ConcurrentDictionary<Type, object?>> Subscribers = new();

    public void Subscribe<TEvent>(object subscriber) where TEvent : notnull
    {
        if (subscriber is not IAsyncEventHandler<TEvent>)
        {
            throw new Exception($"{subscriber.GetType()} does not implement {typeof(IAsyncEventHandler<TEvent>)}");
        }

        var subscribers = Events.GetOrAdd(typeof(TEvent), new ConcurrentDictionary<object, object?>());
        subscribers.TryAdd(subscriber, null);
        
        var events = Subscribers.GetOrAdd(subscriber, new ConcurrentDictionary<Type, object?>());
        events.TryAdd(typeof(TEvent), null);
    }

    public void Unsubscribe<TEvent>(object subscriber) where TEvent : notnull
    {
        var subscribers = Events.GetOrAdd(typeof(TEvent), new ConcurrentDictionary<object, object?>());
        subscribers.TryRemove(subscriber, out _);
        
        var events = Subscribers.GetOrAdd(subscriber, new ConcurrentDictionary<Type, object?>());
        events.TryRemove(typeof(TEvent), out _);
    }

    public void Unsubscribe(object subscriber)
    {
        Subscribers.TryRemove(subscriber, out var events);

        if (events is null)
        {
            return;
        }

        foreach (var @event in events)
        {
            var subscribers = Events.GetOrAdd(@event.Key, new ConcurrentDictionary<object, object?>());
            subscribers.TryRemove(subscriber, out _);
        }
    }

    public Task PublishAsync<TEvent>(TEvent e) where TEvent : notnull
    {
        Log.Debug("{Event} was published", typeof(TEvent));
        
        if (Events.TryGetValue(typeof(TEvent), out var events))
        {
            return events.ForEachAsync(ev =>
            {
                if (ev.Key is IAsyncEventHandler<TEvent> asyncEventHandler)
                {
                    Log.Debug("Calling {Handler} to handle {Event}", asyncEventHandler.GetType().FullName, typeof(TEvent));
                    MainThread.BackgroundFireAndForget(() => asyncEventHandler.HandleAsync(e), DispatcherPriority.Send);
                    return Task.CompletedTask;
                }

                return Task.CompletedTask;
            });
        }

        return Task.CompletedTask;
    }
}