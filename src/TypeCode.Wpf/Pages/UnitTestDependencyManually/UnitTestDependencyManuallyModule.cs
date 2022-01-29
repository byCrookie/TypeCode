using Jab;

namespace TypeCode.Wpf.Pages.UnitTestDependencyManually;

[ServiceProviderModule]
[Transient(typeof(UnitTestDependencyManuallyView))]
[Transient(typeof(UnitTestDependencyManuallyViewModel))]
public partial interface IUnitTestDependencyManuallyModule
{
}