namespace TypeCode.Business.Mode.Specflow;

public interface ITableGenerator
{
    string Build(List<List<string>> rows);
}