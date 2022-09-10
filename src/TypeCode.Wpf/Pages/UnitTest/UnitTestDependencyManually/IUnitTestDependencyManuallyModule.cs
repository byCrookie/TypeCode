using Jab;

namespace TypeCode.Wpf.Pages.UnitTest.UnitTestDependencyManually;

[ServiceProviderModule]
[Transient(typeof(UnitTestDependencyManuallyView))]
[Transient(typeof(UnitTestDependencyManuallyViewModel))]
public interface IUnitTestDependencyManuallyModule
{
    
}