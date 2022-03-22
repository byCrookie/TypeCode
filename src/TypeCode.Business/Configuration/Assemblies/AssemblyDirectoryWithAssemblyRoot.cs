namespace TypeCode.Business.Configuration.Assemblies;

public class AssemblyDirectoryWithAssemblyRoot
{
    public AssemblyDirectoryWithAssemblyRoot(AssemblyRoot assemblyRoot, AssemblyDirectory assemblyDirectory)
    {
        AssemblyRoot = assemblyRoot;
        AssemblyDirectory = assemblyDirectory;
    }

    public AssemblyRoot AssemblyRoot { get; }
    public AssemblyDirectory AssemblyDirectory { get; }
}