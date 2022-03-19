using System.Reflection;

namespace TypeCode.Business.Configuration;

public class AssemblyFileLoader : IAssemblyFileLoader
{
    public Task<Assembly> LoadAsync(string path)
    {
        return LoadFromPathAsync(path);
    }

    private static async Task<Assembly> LoadFromPathAsync(string path)
    {
        var fullPath = Path.GetFullPath(path);
        
        await using (var fs = new FileStream(fullPath, FileMode.Open))
        {
            return new CustomAssemblyLoadContext(fullPath).LoadFromStream(fs);
        }
    }
}