using Jab;
using TypeCode.Wpf.Pages.Builder;

namespace TypeCode.Wpf.Pages.Home;

[ServiceProviderModule]
[Transient(typeof(HomeView))]
[Transient(typeof(HomeViewModel))]
public interface IHomeModule
{
    
}