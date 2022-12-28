namespace TypeCode.Business.Mode.UnitTestDependency.Manually;

internal class DependencyManually
{
    public DependencyManually(string dependency)
    {
        var splitted = new ExclusionSplitter(" ", "<", ">").Split(dependency).ToList();
        TypeName = splitted[0];
        Name = splitted[1];
    }

    public string Name { get; }
    public string TypeName { get; }
}