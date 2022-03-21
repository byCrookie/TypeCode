namespace TypeCode.Business.Configuration.Assemblies;

internal interface IAssemblyHolder
{
    string Path { get; set; }
    List<AssemblyDirectory> AssemblyDirectories { get; set; }
}