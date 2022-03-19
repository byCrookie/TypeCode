﻿using System.Reflection;
using System.Runtime.Loader;
using System.Security.Cryptography;
using System.Text;

namespace TypeCode.Business.Configuration;

public class AssemblyFileLoader : IAssemblyFileLoader
{
    public Task<Assembly> LoadAsync(string path)
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

        return LoadFromPathAsync(cachedAssembly);
    }

    private static async Task<Assembly> LoadFromPathAsync(string path)
    {
        await using (var fs = new FileStream(Path.GetFullPath(path), FileMode.Open))
        {
            return new AssemblyLoadContext(Guid.NewGuid().ToString("N")).LoadFromStream(fs);
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