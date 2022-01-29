using Framework.Jab.Boot;
using Jab;
using TypeCode.Console.Mode;
using Workflow;

namespace TypeCode.Console.Modules;

[ServiceProviderModule]
[Import(typeof(IModeModule))]
[Transient(typeof(IWorkflowBuilder<TypeCodeContext>), typeof(WorkflowBuilder<TypeCodeContext>))]
[Singleton(typeof(IApplication), typeof(TypeCode))]
internal partial interface ITypeCodeConsoleModule
{
}