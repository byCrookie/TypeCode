namespace TypeCode.Business.Logging;

public static class LogFilePaths
{
    public const string File = @"logs\TypeCode.log.txt";
    public const string FileFatal = @"logs\TypeCode.Fatal.log.txt";
    public const string IndexedTypes = @"logs\TypeCode.IndexedTypes.log.txt";

    public static IEnumerable<string> AllPaths => new List<string>
    {
        File,
        FileFatal,
        IndexedTypes
    };
}