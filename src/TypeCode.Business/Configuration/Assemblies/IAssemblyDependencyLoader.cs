using System.Reflection;

namespace TypeCode.Business.Configuration.Assemblies;

public interface IAssemblyDependencyLoader
{
    Task<Assembly?> LoadFromAssemblyPathAsync(AssemblyDirectoryWithAssemblyRoot assemblyDirectory, string assemblyFullPath);
}