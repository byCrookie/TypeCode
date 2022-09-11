using Jab;

namespace TypeCode.Wpf.Pages.UnitTest.UnitTestDependencyManually;

[ServiceProviderModule]
[Singleton(typeof(UnitTestDependencyManuallyView))]
[Singleton(typeof(UnitTestDependencyManuallyViewModel))]
public interface IUnitTestDependencyManuallyModule
{
    
}