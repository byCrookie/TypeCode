﻿using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Serilog;
using TypeCode.Business.TypeEvaluation;

namespace TypeCode.Business.Configuration.Assemblies;

public class AssemblyLoader : IAssemblyLoader
{
    public async Task LoadAsync(TypeCodeConfiguration configuration)
    {
        const string cacheDirectory = "cache";
        Directory.CreateDirectory(cacheDirectory);
        
        var assemblyDirectories = configuration.AssemblyRoot
            .SelectMany(root => root.AssemblyGroup
                .SelectMany(group => group.AssemblyPath
                    .SelectMany(path => path.AssemblyDirectories)
                    .Concat(group.AssemblyPathSelector
                        .SelectMany(selector => selector.AssemblyDirectories))));

        await Parallel.ForEachAsync(assemblyDirectories, async (assemblyDirectory, _) =>
        {
            await Parallel.ForEachAsync(assemblyDirectory.AssemblyCompounds, _, async (assemblyCompound, _) =>
            {
               assemblyCompound.Assembly = await LoadAsync(assemblyCompound.File).ConfigureAwait(false);
               assemblyCompound.Types = LoadTypes(assemblyCompound);
            }).ConfigureAwait(false);
        }).ConfigureAwait(false);

        Log.Debug("Total of {0} assemblies have been loaded", CountAssemblies(configuration));
    }
    
    private static Task<Assembly?> LoadAsync(string path)
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
            return Task.FromResult<Assembly?>(assembly);
        }
        catch (Exception exception)
        {
            Log.Warning("Error loading assembly {Assembly}: {Exception}", path, exception.Message);
            return Task.FromResult<Assembly?>(null);
        }
    }
    
    private static List<Type> LoadTypes(AssemblyCompound assemblyCompound)
    {
        var loadedTypes = assemblyCompound.Assembly?.GetLoadableTypes().ToList();
        Log.Debug("Loaded {Count} Types from {Assembly}", loadedTypes?.Count, assemblyCompound.Assembly?.FullName);
        return loadedTypes ?? new List<Type>();
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

    private static int CountAssemblies(TypeCodeConfiguration configuration)
    {
        var counter = 0;
        foreach (var assemblyGroup in configuration.AssemblyRoot.SelectMany(assemblyRoot => assemblyRoot.AssemblyGroup))
        {
            counter += assemblyGroup.AssemblyPath
                .SelectMany(assemblyPath => assemblyPath.AssemblyDirectories)
                .Sum(assemblyDirectory => assemblyDirectory.AssemblyCompounds.Select(com => com.Assembly).Count());
            counter += assemblyGroup.AssemblyPath
                .SelectMany(assemblyPath => assemblyPath.AssemblyDirectories)
                .Sum(assemblyDirectory => assemblyDirectory.AssemblyCompounds.Select(com => com.Assembly).Count());
        }

        return counter;
    }
}