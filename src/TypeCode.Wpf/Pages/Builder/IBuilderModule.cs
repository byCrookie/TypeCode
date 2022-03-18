using Jab;

namespace TypeCode.Wpf.Pages.Builder;

[ServiceProviderModule]
[Transient(typeof(BuilderView))]
[Transient(typeof(BuilderViewModel))]
public interface IBuilderModule
{
    
}