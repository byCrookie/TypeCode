using Jab;

namespace TypeCode.Wpf.Pages.Specflow;

[ServiceProviderModule]
[Singleton(typeof(SpecflowView))]
[Singleton(typeof(SpecflowViewModel))]
public interface ISpecflowModule
{
    
}