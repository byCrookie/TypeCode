namespace TypeCode.Business.Configuration.Assemblies;

public sealed class AssemblyPriorityString
{
    public AssemblyPriorityString(string priority, string assembly, bool ignore)
    {
        Priority = priority;
        Assembly = assembly;
        Ignore = ignore;
    }
    
    public string Priority { get; }
    public string Assembly { get; }
    public bool Ignore { get; }

    public override string ToString()
    {
        return $"{nameof(Priority)}:{Priority} - {nameof(Assembly)}:{Assembly} - {nameof(Ignore)}:{Ignore}";
    }
}