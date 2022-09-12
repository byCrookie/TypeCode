namespace TypeCode.Business.Mode.UnitTestDependency.Type;

public sealed class UnitTestDependencyTypeGeneratorParameter : ITypeCodeGeneratorParameter
{
    public UnitTestDependencyTypeGeneratorParameter()
    {
        Types = new List<System.Type>();
    }
    
    public List<System.Type> Types { get; set; }
}