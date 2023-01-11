namespace TypeCode.Business.Tests;

public sealed class TestClass
{
    public TestClass()
    {
        String = string.Empty;
        SubTestClasses = new List<Test1.TestClass>();
        List = new List<string>();
        Enumerable = System.Linq.Enumerable.Empty<string>();
    }
    
    public string? NullableString { get; set; }
    public string String { get; set; }
    public double? NullableDouble { get; set; }
    public double Double { get; set; }
    public DateTime? NullableDateTime { get; set; }
    public DateTime DateTimeValue { get; set; }
    public TestEnum TestEnumValue { get; set; }
    public List<TypeCode.Business.Tests.Test1.TestClass> SubTestClasses { get; set; }
    public TypeCode.Business.Tests.Test1.TestClass? SubTestClass { get; set; }
    public TypeCode.Business.Tests.Test1.TestClass? SubTestClass2 { get; set; }
    public IEnumerable<string> Enumerable { get; set; }
    public List<string> List { get; set; }
}