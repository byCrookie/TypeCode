namespace TypeCode.Business.Version;

public interface IVersionEvaluator
{
    Task<string?> EvaluateAsync();
}