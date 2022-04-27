namespace TypeCode.Wpf.Pages.DynamicExecute.Code;

public interface IRunner
{
    string Execute(byte[] compiledAssembly);
}