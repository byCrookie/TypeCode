using Jab;

namespace TypeCode.Business.Mode.DynamicExecution;

[ServiceProviderModule]
[Transient(typeof(ICompiler), typeof(Compiler))]
[Transient(typeof(IRunner), typeof(Runner))]
public interface IDynamicExecutionModule
{
    
}