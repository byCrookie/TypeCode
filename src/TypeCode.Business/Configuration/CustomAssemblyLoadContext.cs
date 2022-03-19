using System.Runtime.Loader;

namespace TypeCode.Business.Configuration;

public class CustomAssemblyLoadContext : AssemblyLoadContext
{
    public CustomAssemblyLoadContext() : base(Guid.NewGuid().ToString("N"))
    {
        
    }
}