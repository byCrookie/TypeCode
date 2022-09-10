using Jab;

namespace TypeCode.Business.Version;

[ServiceProviderModule]
[Transient(typeof(IVersionEvaluator), typeof(VersionEvaluator))]
[Transient(typeof(ISemanticVersionComparer), typeof(SemanticVersionComparer))]
public interface IVersionModule
{
}