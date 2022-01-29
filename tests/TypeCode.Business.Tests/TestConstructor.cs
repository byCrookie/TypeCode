namespace TypeCode.Business.Tests
{
    public class TestConstructor
    {
        private readonly ITestInterface _testInterface;

        public TestConstructor(
            ITestInterface testInterface
        )
        {
            _testInterface = testInterface;
        }
    }
}