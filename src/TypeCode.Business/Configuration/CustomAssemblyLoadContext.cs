using System.Runtime.Loader;

namespace TypeCode.Business.Configuration;

public class CustomAssemblyLoadContext : AssemblyLoadContext
{
    public CustomAssemblyLoadContext(string path) : base(path, true)
    {
    }
}