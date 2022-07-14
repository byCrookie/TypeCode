using System.Text;

namespace TypeCode.Business.Mode.Specflow;

public class TableGenerator : ITableGenerator
{
    public string Build(List<List<string>> rows)
    {
        var invertedRows = Invert(rows);

        foreach (var column in invertedRows.ToList())
        {
            var maxColumnLength = column.Max(value => value?.Length ?? 0);

            foreach (var valueIndex in column.Select((v, i) => new { Value = v, Index = i }).ToList())
            {
                column[valueIndex.Index] = valueIndex.Value?.PadRight(maxColumnLength) ?? string.Empty.PadRight(maxColumnLength);
            }
        }

        var table = new StringBuilder();
        foreach (var row in Invert(invertedRows))
        {
            table.AppendLine($"| {string.Join(" | ", row)} |");
        }

        return table.ToString();
    }

    private static List<List<T>> Invert<T>(IReadOnlyList<List<T>> rows)
    {
        var inverted = new T[rows[0].Count][];

        for (var i = 0; i < rows[0].Count; i++)
        {
            inverted[i] = new T[rows.Count];
        }

        foreach (var rowIndex in rows.Select((r, i) => new { Row = r, Index = i }))
        {
            foreach (var valueIndex in rowIndex.Row.Select((v, i) => new { Value = v, Index = i }))
            {
                inverted[valueIndex.Index][rowIndex.Index] = valueIndex.Value;
            }
        }

        return inverted.Select(inner => inner.ToList()).ToList();
    }
}