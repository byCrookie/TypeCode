using System.Threading.Tasks;

namespace TypeCode.Wpf.Helper.Event;

public interface IAsyncEventHandler<in TEvent>
{
    public Task HandleAsync(TEvent e);
}