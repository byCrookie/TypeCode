using System.Runtime.Loader;

namespace TypeCode.Business.Configuration.Assemblies;

public sealed class CustomAssemblyLoadContext : AssemblyLoadContext
{
    public CustomAssemblyLoadContext(string path) : base(path, true)
    {
    }
}