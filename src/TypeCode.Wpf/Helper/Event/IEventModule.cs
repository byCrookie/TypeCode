using Jab;

namespace TypeCode.Wpf.Helper.Event;

[ServiceProviderModule]
[Singleton(typeof(IEventAggregator), typeof(EventAggregator))]
public partial interface IEventModule
{
}