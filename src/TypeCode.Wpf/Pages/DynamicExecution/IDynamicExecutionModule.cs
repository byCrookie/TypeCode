using Jab;

namespace TypeCode.Wpf.Pages.DynamicExecution;

[ServiceProviderModule]
[Transient(typeof(DynamicExecutionView))]
[Transient(typeof(DynamicExecutionViewModel))]
public interface IDynamicExecutionModule
{
    
}