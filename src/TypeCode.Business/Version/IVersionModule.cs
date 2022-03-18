using Jab;

namespace TypeCode.Business.Version;

[ServiceProviderModule]
[Transient(typeof(IVersionEvaluator), typeof(VersionEvaluator))]
public interface IVersionModule
{
}