namespace TypeCode.Business.Mode.DynamicExecution;

public interface ICompiler
{
    byte[] Compile(string sourceCode);
}