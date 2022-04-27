using Jab;

namespace TypeCode.Wpf.Pages.DynamicExecute;

[ServiceProviderModule]
[Transient(typeof(DynamicExecuteView))]
[Transient(typeof(DynamicExecuteViewModel))]
public interface IDynamicExecuteModule
{
    
}