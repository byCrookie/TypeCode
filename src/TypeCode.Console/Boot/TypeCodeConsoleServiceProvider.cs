using Framework.Jab;
using Framework.Jab.Boot;
using Jab;
using TypeCode.Business.Bootstrapping;
using TypeCode.Business.Modules;
using TypeCode.Console.Modules;
using Workflow;

namespace TypeCode.Console.Boot;

[ServiceProvider]
[Transient(typeof(IWorkflowBuilder<BootContext>), typeof(WorkflowBuilder<BootContext>))]
[Import(typeof(IFrameworkModule))]
[Transient(typeof(IAssemblyLoadBootStep<>), typeof(AssemblyLoadBootStep<>))]
[Import(typeof(ITypeCodeConsoleModule))]
[Import(typeof(ITypeCodeBusinessModule))]
public partial class TypeCodeConsoleServiceProvider
{
}