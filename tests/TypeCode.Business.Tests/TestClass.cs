namespace TypeCode.Business.Tests;

public sealed class TestClass
{
    public TestClass()
    {
        String = string.Empty;
        SubTestClasses = new List<Test1.TestClass>();
    }
    
    public string? NullableString { get; set; }
    public string String { get; set; }
    public double? NullableDouble { get; set; }
    public double Double { get; set; }
    public DateTime? NullableDateTime { get; set; }
    public DateTime DateTime { get; set; }
    public List<TypeCode.Business.Tests.Test1.TestClass> SubTestClasses { get; set; }
    public TypeCode.Business.Tests.Test1.TestClass? SubTestClass { get; set; }
    public TypeCode.Business.Tests.Test1.TestClass? SubTestClass2 { get; set; }
}