using Jab;

namespace TypeCode.Business.Embedded;

[ServiceProviderModule]
[Transient(typeof(IResourceReader), typeof(ResourceReader))]
public interface IEmbeddedModule
{
}