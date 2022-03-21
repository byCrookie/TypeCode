using System.Reflection;
using System.Runtime.Loader;

namespace TypeCode.Business.Configuration.Assemblies;

public interface IAssemblyDependencyLoader
{
    Assembly? LoadFromAssemblyPath(AssemblyLoadContext assemblyLoadContext, string assemblyFullPath);
}