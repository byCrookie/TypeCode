namespace TypeCode.Business.Tests.Test2
{
    public class TestConstructor
    {
        private readonly ITestInterface _testInterface;

        public TestConstructor(ITestInterface testInterface)
        {
            _testInterface = testInterface;
        }
    }
}