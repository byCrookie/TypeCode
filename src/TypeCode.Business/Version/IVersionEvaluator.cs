namespace TypeCode.Business.Version;

public interface IVersionEvaluator
{
    Task<VersionResult> EvaluateAsync();
    Task<VersionResult> ReadCurrentVersionAsync();
}