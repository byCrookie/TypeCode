using Jab;

namespace TypeCode.Wpf.Pages.Home;

[ServiceProviderModule]
[Transient(typeof(HomeView))]
[Transient(typeof(HomeViewModel))]
public interface IHomeModule
{
    
}