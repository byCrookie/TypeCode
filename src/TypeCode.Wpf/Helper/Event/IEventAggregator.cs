using System.Threading.Tasks;

namespace TypeCode.Wpf.Helper.Event;

public interface IEventAggregator
{
    public void Subscribe<TEvent>(object subscriber);
    public void Unsubscribe<TEvent>(object subscriber);
    public Task PublishAsync<TEvent>(TEvent e);
}