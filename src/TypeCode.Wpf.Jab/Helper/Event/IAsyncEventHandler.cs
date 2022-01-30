using System.Threading.Tasks;

namespace TypeCode.Wpf.Jab.Helper.Event;

public interface IAsyncEventHandler<in TEvent>
{
    public Task HandleAsync(TEvent e);
}