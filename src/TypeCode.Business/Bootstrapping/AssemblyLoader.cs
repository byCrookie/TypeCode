using System.Reflection;
using Serilog;

namespace TypeCode.Business.Bootstrapping;

internal class AssemblyLoader : IAssemblyLoader
{
    public Task<Assembly> LoadAsync(string path)
    {
        const string cacheDirectory = "cache";
        Directory.CreateDirectory(cacheDirectory);

        var fileName = Path.GetFileNameWithoutExtension(path);
        var directoryPath = Path.GetDirectoryName(path) ?? string.Empty;

        var cacheDirectoryPath = Path.Combine(cacheDirectory,
            directoryPath.Replace(":", "").Replace("\\", "").Replace(".", ""));
        Directory.CreateDirectory(cacheDirectoryPath);

        var cachedAssembly = Path.Combine(cacheDirectoryPath, $"{fileName}.dll");

        if (!File.Exists(cachedAssembly) || AssemblyIsNewer(path, cachedAssembly))
        {
            File.Copy(path, cachedAssembly, true);
        }

        try
        {
            var assembly = Assembly.LoadFrom(cachedAssembly);
            return Task.FromResult(assembly);
        }
        catch (Exception e)
        {
            Log.Warning("{0}", e.Message);
            throw;
        }
    }

    private static bool AssemblyIsNewer(string path, string cachedAssembly)
    {
        return File.GetLastWriteTime(path) > File.GetLastWriteTime(cachedAssembly);
    }
}