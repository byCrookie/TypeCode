using Jab;
using TypeCode.Wpf.Pages.UnitTest.UnitTestDependencyManually;
using TypeCode.Wpf.Pages.UnitTest.UnitTestDependencyType;

namespace TypeCode.Wpf.Pages.UnitTest;

[ServiceProviderModule]
[Import(typeof(IUnitTestDependencyTypeModule))]
[Import(typeof(IUnitTestDependencyManuallyModule))]
[Transient(typeof(UnitTestView))]
[Transient(typeof(UnitTestViewModel))]
public interface IUnitTestModule
{
    
}