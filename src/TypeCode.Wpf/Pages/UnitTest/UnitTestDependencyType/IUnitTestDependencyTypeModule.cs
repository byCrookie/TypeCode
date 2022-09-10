using Jab;

namespace TypeCode.Wpf.Pages.UnitTest.UnitTestDependencyType;

[ServiceProviderModule]
[Transient(typeof(UnitTestDependencyTypeView))]
[Transient(typeof(UnitTestDependencyTypeViewModel))]
public interface IUnitTestDependencyTypeModule
{
    
}