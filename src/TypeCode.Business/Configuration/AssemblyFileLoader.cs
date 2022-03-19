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
        await using (var fs = new FileStream(Path.GetFullPath(path), FileMode.Open))
        {
            return new CustomAssemblyLoadContext().LoadFromStream(fs);
        }
    }
}