using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Serilog;

namespace TypeCode.Business.Configuration;

public class AssemblyFileLoader : IAssemblyFileLoader
{
    public async Task<Assembly> LoadAsync(string path)
    {
        try
        {
            const string cacheDirectory = "cache";
            Directory.CreateDirectory(cacheDirectory);

            var fileName = Path.GetFileNameWithoutExtension(path);
            var directoryPath = Path.GetDirectoryName(path) ?? string.Empty;
            
            var cacheDirectoryPath = Path.Combine(cacheDirectory, GetHashString(directoryPath));
            Directory.CreateDirectory(cacheDirectoryPath);
            
            var directoryCacheDescriptionFile = Path.Combine(cacheDirectoryPath, "_origin.txt");

            if (!File.Exists(directoryCacheDescriptionFile))
            {
                await File.AppendAllTextAsync(directoryCacheDescriptionFile, directoryPath).ConfigureAwait(false);
            }

            var cachedAssembly = Path.Combine(cacheDirectoryPath, $"{fileName}.dll");

            if (!File.Exists(cachedAssembly) || AssemblyIsNewer(path, cachedAssembly))
            {
                try
                {
                    File.Copy(path, cachedAssembly, true);
                }
                catch (Exception e)
                {
                    Log.Warning("Cache-Error. Cache create not successful from {From} to {To}: {Exception}", path, cachedAssembly, e.Message);
                    var randomCacheAssembly = Path.Combine(cacheDirectoryPath, $"{Guid.NewGuid():N}_{fileName}.dll");
                    File.Copy(path, randomCacheAssembly, true);
                    return Assembly.LoadFrom(randomCacheAssembly);
                }
            }
            
            return Assembly.LoadFrom(cachedAssembly);
        }
        catch (Exception e)
        {
            Log.Warning("Cache-Error. Ignore Cache: {Exception}", e.Message);
            return Assembly.LoadFrom(path);
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