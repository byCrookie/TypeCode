using static TypeCode.Business.Mode.Specflow.TableFormatter;

namespace TypeCode.Business.Mode.Specflow;

public interface ITableFormatter
{
    string Format(IEnumerable<IEnumerable<string?>> rows, TableFormatOptions? options = null, params string[] headers);
    string Format(IEnumerable<IEnumerable<string?>> rows, IEnumerable<string?> headers, TableFormatOptions? options = null);
    string Format(IEnumerable<IEnumerable<string?>> rows, TableFormatOptions? options = null);
}