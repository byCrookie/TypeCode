using Jab;

namespace TypeCode.Wpf.Pages.UnitTestDependencyManually;

[ServiceProviderModule]
[Transient(typeof(UnitTestDependencyManuallyView))]
[Transient(typeof(UnitTestDependencyManuallyViewModel))]
public interface IUnitTestDependencyManuallyModule
{
    
}