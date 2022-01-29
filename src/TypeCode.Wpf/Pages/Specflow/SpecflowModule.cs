using Jab;

namespace TypeCode.Wpf.Pages.Specflow;

[ServiceProviderModule]
[Transient(typeof(SpecflowView))]
[Transient(typeof(SpecflowViewModel))]
public partial interface ISpecflowModule
{
}