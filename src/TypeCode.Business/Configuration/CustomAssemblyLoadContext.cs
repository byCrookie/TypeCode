using System.Runtime.Loader;

namespace TypeCode.Business.Configuration;

public class CustomAssemblyLoadContext : AssemblyLoadContext
{
    // private readonly AssemblyDependencyResolver _resolver;
    
    public CustomAssemblyLoadContext(string path) : base(path, true)
    {
        // _resolver = new AssemblyDependencyResolver(path);
    }
    
    // protected override Assembly? Load(AssemblyName assemblyName)
    // {
    //     var assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);
    //
    //     if (assemblyPath is null || !File.Exists(assemblyPath))
    //     {
    //         return null;
    //     }
    //
    //     using (var fs = new FileStream(assemblyPath, FileMode.Open))
    //     {
    //         return LoadFromStream(fs);
    //     }
    // }
    //
    // protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
    // {
    //     var libraryPath = _resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
    //     return libraryPath is not null ? LoadUnmanagedDllFromPath(libraryPath) : IntPtr.Zero;
    // }
}