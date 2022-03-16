using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Serilog;

namespace TypeCode.Business.Bootstrapping;

internal class AssemblyLoader : IAssemblyLoader
{
    public Task<Assembly> LoadAsync(string path)
    {
        try
        {
            const string cacheDirectory = "cache";
            Directory.CreateDirectory(cacheDirectory);

            var fileName = Path.GetFileNameWithoutExtension(path);
            var directoryPath = Path.GetDirectoryName(path) ?? string.Empty;

            var cacheDirectoryPath = Path.Combine(cacheDirectory, GetHashString(directoryPath));
            Directory.CreateDirectory(cacheDirectoryPath);

            var cachedAssembly = Path.Combine(cacheDirectoryPath, $"{fileName}.dll");

            if (!File.Exists(cachedAssembly) || AssemblyIsNewer(path, cachedAssembly))
            {
                File.Copy(path, cachedAssembly, true);
            }
            
            var assembly = Assembly.LoadFrom(cachedAssembly);
            return Task.FromResult(assembly);
        }
        catch (Exception e)
        {
            Log.Warning("Cache-Error. Ignore Cache: {Exception}", e.Message);
            var assembly = Assembly.LoadFrom(path);
            return Task.FromResult(assembly);
        }
    }

    private static bool AssemblyIsNewer(string path, string cachedAssembly)
    {
        return File.GetLastWriteTime(path) > File.GetLastWriteTime(cachedAssembly);
    }

    private static string GetHashString(string inputString)
    {
        var sb = new StringBuilder();
        foreach (var b in GetHash(inputString))
        {
            sb.Append(b.ToString("X2"));
        }

        return sb.ToString();
    }

    private static IEnumerable<byte> GetHash(string inputString)
    {
        using (HashAlgorithm algorithm = SHA1.Create())
        {
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }
    }
}