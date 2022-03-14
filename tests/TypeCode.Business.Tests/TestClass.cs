namespace TypeCode.Business.Tests;

public class TestClass
{
    public TestClass()
    {
        String = string.Empty;
    }
    
    public string? NullableString { get; set; }
    public string String { get; set; }
    public double? NullableDouble { get; set; }
    public double Double { get; set; }
    public DateTime? NullableDateTime { get; set; }
    public DateTime DateTime { get; set; }
}