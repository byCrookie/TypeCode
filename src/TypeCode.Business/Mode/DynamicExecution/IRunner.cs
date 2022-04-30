namespace TypeCode.Business.Mode.DynamicExecution;

public interface IRunner
{
    string Execute(byte[] compiledAssembly);
}