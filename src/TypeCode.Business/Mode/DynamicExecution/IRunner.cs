namespace TypeCode.Business.Mode.DynamicExecution;

public interface IRunner
{
    string CompileAndExecute(string sourceCode, params string?[] parameters);
    string Execute(byte[] compiledAssembly, params string?[] parameters);
}