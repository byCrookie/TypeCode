using Jab;

namespace TypeCode.Wpf.Pages.Builder;

[ServiceProviderModule]
[Singleton(typeof(BuilderView))]
[Singleton(typeof(BuilderViewModel))]
public interface IBuilderModule
{
    
}