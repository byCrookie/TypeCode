namespace TypeCode.Business.Tests;

public sealed class TestConstructor
{
    private readonly ITestInterface _testInterface;

    public TestConstructor(
        ITestInterface testInterface
    )
    {
        _testInterface = testInterface;
    }
}