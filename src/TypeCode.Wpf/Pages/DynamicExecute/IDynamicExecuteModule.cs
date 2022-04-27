using Jab;
using TypeCode.Wpf.Pages.DynamicExecute.Code;

namespace TypeCode.Wpf.Pages.DynamicExecute;

[ServiceProviderModule]
[Transient(typeof(DynamicExecuteView))]
[Transient(typeof(DynamicExecuteViewModel))]
[Transient(typeof(ICompiler), typeof(Compiler))]
[Transient(typeof(IRunner), typeof(Runner))]
public interface IDynamicExecuteModule
{
    
}