namespace TypeCode.Business.Logging;

public static class LogFiles
{
    public const string File = "TypeCode.log.txt";
    public const string FileFatal = "TypeCode.Fatal.log.txt";
    public const string IndexedTypes = "TypeCode.IndexedTypes.log.txt";

    public static IEnumerable<string> All => new List<string>
    {
        File,
        FileFatal,
        IndexedTypes
    };
}