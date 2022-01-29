using Jab;

namespace TypeCode.Wpf.Pages.Assemblies;

[ServiceProviderModule]
[Transient(typeof(AssemblyView))]
[Transient(typeof(AssemblyViewModel))]
public partial interface IAssemblyModule
{
}