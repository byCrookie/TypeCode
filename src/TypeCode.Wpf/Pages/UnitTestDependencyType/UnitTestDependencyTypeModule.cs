using Jab;

namespace TypeCode.Wpf.Pages.UnitTestDependencyType;

[ServiceProviderModule]
[Transient(typeof(UnitTestDependencyTypeView))]
[Transient(typeof(UnitTestDependencyTypeViewModel))]
public partial interface IUnitTestDependencyTypeModule
{
}