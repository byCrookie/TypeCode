using System.Text;
using System.Text.RegularExpressions;

namespace TypeCode.Business.Mode.Specflow;

public sealed class TableFormatter : ITableFormatter
{
    public string Format(IEnumerable<IEnumerable<string?>> rows, TableFormatOptions? options = null, params string[] headers)
    {
        return Format(rows, headers.ToList(), options);
    }
    
    public string Format(IEnumerable<IEnumerable<string?>> rows, IEnumerable<string?> headers, TableFormatOptions? options = null)
    {
        var paddedValues = PadValues(rows.Prepend(headers).Select(inner => inner.ToArray()).ToArray());

        var table = new StringBuilder();

        if (options == TableFormatOptions.HeaderSeperator)
        {
            table.AppendLine($"| {string.Join(" | ", paddedValues.First())} |");
            table.AppendLine("| " + string.Join(" | ", CreateTableHeaderBodySeperator(paddedValues)) + " |");
        }

        foreach (var row in paddedValues)
        {
            table.AppendLine($"| {string.Join(" | ", row)} |");
        }

        return table.ToString();
    }

    public string Format(IEnumerable<IEnumerable<string?>> rows, TableFormatOptions? options = null)
    {
        var paddedArray = PadValues(rows.Select(inner => inner.ToArray()).ToArray());
        var table = new StringBuilder();
        foreach (var row in paddedArray)
        {
            table.AppendLine($"| {string.Join(" | ", row)} |");
        }

        return table.ToString();
    }
    
    private static IEnumerable<string> CreateTableHeaderBodySeperator(IReadOnlyList<string?[]> paddedValues) {
        return paddedValues[0].Select(value => value is null 
            ? throw new ArgumentNullException(value) 
            : new Regex("(.)").Replace(value, "-"));
    }
    
    private static string?[][] PadValues(IReadOnlyList<string?[]> rows)
    {
        var transpondedArray = Transpond(rows);

        foreach (var column in transpondedArray)
        {
            var maxColumnWidth = column.Max(value => value?.Length ?? 0);

            for (var rowIndex = 0; rowIndex < column.Length; rowIndex++)
            {
                column[rowIndex] = column[rowIndex]?.PadRight(maxColumnWidth) ?? string.Empty.PadRight(maxColumnWidth);
            }
        }

        return Transpond(transpondedArray);
    }

    private static T[][] Transpond<T>(IReadOnlyList<T[]> rows)
    {
        var transpondedArray = new T[rows[0].Length][];

        for (var i = 0; i < rows[0].Length; i++)
        {
            transpondedArray[i] = new T[rows.Count];
        }

        for (var rowIndex = 0; rowIndex < rows.Count; rowIndex++)
        {
            for (var valueIndex = 0; valueIndex < rows[rowIndex].Length; valueIndex++)
            {
                transpondedArray[valueIndex][rowIndex] = rows[rowIndex][valueIndex];
            }
        }

        return transpondedArray;
    }
    
    [Flags]
    public enum TableFormatOptions
    {
        HeaderSeperator
    }
}