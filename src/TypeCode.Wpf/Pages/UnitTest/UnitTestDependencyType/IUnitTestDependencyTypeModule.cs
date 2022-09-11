using Jab;

namespace TypeCode.Wpf.Pages.UnitTest.UnitTestDependencyType;

[ServiceProviderModule]
[Singleton(typeof(UnitTestDependencyTypeView))]
[Singleton(typeof(UnitTestDependencyTypeViewModel))]
public interface IUnitTestDependencyTypeModule
{
    
}