namespace TypeCode.Business.Version;

public interface ISemanticVersionComparer
{
    bool IsNewer(string current, string newest);
}