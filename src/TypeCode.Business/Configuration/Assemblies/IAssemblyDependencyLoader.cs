using System.Reflection;
using System.Runtime.Loader;

namespace TypeCode.Business.Configuration.Assemblies;

public interface IAssemblyDependencyLoader
{
    Task<Assembly?> LoadFromAssemblyPathAsync(AssemblyLoadContext assemblyLoadContext, string assemblyFullPath);
}