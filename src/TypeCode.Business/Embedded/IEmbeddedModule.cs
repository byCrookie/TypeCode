using Jab;
using TypeCode.Business.Mode;
using TypeCode.Business.Mode.Builder;
using TypeCode.Business.Mode.Composer;
using TypeCode.Business.Mode.Mapper;
using TypeCode.Business.Mode.Mapper.Style;
using TypeCode.Business.Mode.Specflow;
using TypeCode.Business.Mode.UnitTestDependency.Manually;
using TypeCode.Business.Mode.UnitTestDependency.Type;

namespace TypeCode.Business.Embedded;

[ServiceProviderModule]
[Transient(typeof(IResourceReader), typeof(ResourceReader))]
public interface IEmbeddedModule
{
}