namespace TypeCode.Business.Mode.Composer;

public sealed class ComposerType
{
    public ComposerType(Type type, List<Type> interfaces)
    {
        Type = type;
        Interfaces = interfaces;
    }
    
    public Type Type { get; }
    public List<Type> Interfaces { get; }
}