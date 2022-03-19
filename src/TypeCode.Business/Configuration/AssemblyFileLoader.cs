using System.Reflection;
using System.Runtime.Loader;
using Serilog;

namespace TypeCode.Business.Configuration;

public class AssemblyFileLoader : IAssemblyFileLoader
{
    public Task<Assembly> LoadAsync(string path)
    {
        Log.Information("Load Assembly {Path}", path);
        return LoadFromPathAsync(path);
    }

    private static async Task<Assembly> LoadFromPathAsync(string path)
    {
        var fullPath = Path.GetFullPath(path);
        
        await using (var fs = new FileStream(fullPath, FileMode.Open))
        {
            return new AssemblyLoadContext(null).LoadFromStream(fs);
        }
    }
}