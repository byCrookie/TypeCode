namespace TypeCode.Wpf.Pages.DynamicExecute.Code;

public interface ICompiler
{
    byte[] Compile(string filepath);
}