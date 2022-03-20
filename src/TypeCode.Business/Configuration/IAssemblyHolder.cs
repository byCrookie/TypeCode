namespace TypeCode.Business.Configuration;

internal interface IAssemblyHolder
{
    string Path { get; set; }
    List<AssemblyDirectory> AssemblyDirectories { get; set; }
}