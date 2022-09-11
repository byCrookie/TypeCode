using Jab;

namespace TypeCode.Wpf.Pages.DynamicExecution;

[ServiceProviderModule]
[Singleton(typeof(DynamicExecutionView))]
[Singleton(typeof(DynamicExecutionViewModel))]
public interface IDynamicExecutionModule
{
    
}