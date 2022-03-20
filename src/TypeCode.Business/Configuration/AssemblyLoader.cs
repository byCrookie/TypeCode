using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.Loader;
using System.Security.Cryptography;
using System.Text;
using Serilog;
using TypeCode.Business.TypeEvaluation;

namespace TypeCode.Business.Configuration;

public class AssemblyLoader : IAssemblyLoader
{
    private readonly ConcurrentDictionary<string, AssemblyCompound> _assemblyCompounds = new();

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

        foreach (var assemblyDirectory in assemblyDirectories)
        {
            foreach (var assemblyCompound in assemblyDirectory.AssemblyCompounds.ToList())
            {
                var fileName = Path.GetFileName(assemblyCompound.File);
                var cacheDirectoryPath = Path.Combine(cacheDirectory, GetHashString(assemblyDirectory.AbsolutPath));
                Directory.CreateDirectory(cacheDirectoryPath);

                var cachedAssembly = Path.Combine(cacheDirectoryPath, fileName);

                if (_assemblyCompounds.ContainsKey(assemblyCompound.File))
                {
                    var loadedCompound = _assemblyCompounds[assemblyCompound.File];
                    assemblyDirectory.AssemblyCompounds.Remove(assemblyCompound);
                    assemblyDirectory.AssemblyCompounds.Add(loadedCompound);
                    
                    if (!File.Exists(cachedAssembly) || AssemblyIsNewer(assemblyCompound.File, cachedAssembly))
                    {
                        Log.Debug("Reload assembly {Path} and refresh cache at {Cache}", assemblyCompound.File, cachedAssembly);
                        loadedCompound.AssemblyLoadContext?.Unload();
                        await UpdateCompoundAsync(loadedCompound, cachedAssembly).ConfigureAwait(false);
                    }
                }
                else
                {
                    Log.Debug("Load assembly {Path} and create cache at {Cache}", assemblyCompound.File, cachedAssembly);
                    await AddCompoundAsync(assemblyCompound, cachedAssembly).ConfigureAwait(false);
                }
            }
        }
        
        Log.Debug("Total of {0} assemblies have been loaded", CountAssemblies(configuration));
    }
    
    private static Task UpdateCompoundAsync(AssemblyCompound assemblyCompound, string cachedAssembly)
    {
        return OverwriteCompoundAsync(assemblyCompound, cachedAssembly);
    }

    private async Task AddCompoundAsync(AssemblyCompound assemblyCompound, string cachedAssembly)
    {
        await OverwriteCompoundAsync(assemblyCompound, cachedAssembly).ConfigureAwait(false);
        _assemblyCompounds.TryAdd(assemblyCompound.File, assemblyCompound);
    }
    
    private static async Task OverwriteCompoundAsync(AssemblyCompound assemblyCompound, string cachedAssembly)
    {
        File.Copy(assemblyCompound.File, cachedAssembly, true);
        var (assemblyLoadContext, assembly) = await LoadFromPathAsync(cachedAssembly).ConfigureAwait(false);
        assemblyCompound.AssemblyLoadContext = assemblyLoadContext;
        assemblyCompound.Assembly = assembly;
        assemblyCompound.Types = LoadTypes(assemblyCompound);
    }

    private static List<Type> LoadTypes(AssemblyCompound assemblyCompound)
    {
        var loadedTypes = assemblyCompound.Assembly?.GetLoadableTypes().ToList();
        Log.Debug("Loaded {Count} Types from {Assembly}", loadedTypes?.Count, assemblyCompound.Assembly?.FullName);
        return loadedTypes ?? new List<Type>();
    }

    private static async Task<(AssemblyLoadContext, Assembly)> LoadFromPathAsync(string path)
    {
        var fullPath = Path.GetFullPath(path);

        await using (var fs = new FileStream(fullPath, FileMode.Open))
        {
            var context = new CustomAssemblyLoadContext(path);
            return (context, context.LoadFromStream(fs));
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