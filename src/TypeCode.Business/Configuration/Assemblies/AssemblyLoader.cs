using System.Collections.Concurrent;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Serilog;
using TypeCode.Business.TypeEvaluation;

namespace TypeCode.Business.Configuration.Assemblies;

public class AssemblyLoader : IAssemblyLoader
{
    private readonly IAssemblyDependencyLoader _assemblyDependencyLoader;
    private readonly ConcurrentDictionary<string, AssemblyDirectory> _assemblyDirectories = new();

    public AssemblyLoader(IAssemblyDependencyLoader assemblyDependencyLoader)
    {
        _assemblyDependencyLoader = assemblyDependencyLoader;
    }

    public async Task LoadAsync(TypeCodeConfiguration configuration)
    {
        const string cacheDirectory = "cache";
        Directory.CreateDirectory(cacheDirectory);
        
        var assemblyDirectories = configuration.AssemblyRoot
            .SelectMany(root => root.AssemblyGroup
                .SelectMany(group => group.AssemblyPath
                    .SelectMany(path => path.AssemblyDirectories.Select(directory => new AssemblyDirectoryWithAssemblyRoot(root, directory)))
                    .Concat(group.AssemblyPathSelector
                        .SelectMany(selector => selector.AssemblyDirectories.Select(directory => new AssemblyDirectoryWithAssemblyRoot(root, directory))))));

        await Parallel.ForEachAsync(assemblyDirectories, async (assemblyDirectory, _) =>
        {
            if (_assemblyDirectories.ContainsKey(assemblyDirectory.AssemblyDirectory.AbsolutPath))
            {
                var loadedAssemblyDirectory = _assemblyDirectories[assemblyDirectory.AssemblyDirectory.AbsolutPath];
                _assemblyDirectories.TryRemove(assemblyDirectory.AssemblyDirectory.AbsolutPath, out var _);
                _assemblyDirectories.TryAdd(assemblyDirectory.AssemblyDirectory.AbsolutPath, assemblyDirectory.AssemblyDirectory);

                if (AnyAssemblyIsNewer(loadedAssemblyDirectory.AssemblyCompounds, assemblyDirectory.AssemblyDirectory.AssemblyCompounds))
                {
                    Log.Debug("Reload assemblies at {Path}", assemblyDirectory.AssemblyDirectory.AbsolutPath);

                    loadedAssemblyDirectory.AssemblyLoadContext?.Unload();
                    assemblyDirectory.AssemblyDirectory.AssemblyLoadContext = new CustomAssemblyLoadContext(assemblyDirectory.AssemblyDirectory.AbsolutPath);

                    await Parallel.ForEachAsync(assemblyDirectory.AssemblyDirectory.AssemblyCompounds, _, (assemblyCompound, _) =>
                    {
                        Log.Debug("Reload assembly at {Path}", assemblyCompound.File);
                        
                        // var assembly = await _assemblyDependencyLoader
                        //     .LoadFromAssemblyPathAsync(assemblyDirectory, assemblyCompound.File)
                        //     .ConfigureAwait(false);
                        // assemblyCompound.Assembly = assembly;
                        // assemblyCompound.Types = LoadTypes(assemblyCompound);
                        // assemblyCompound.LastFileWriteTime = File.GetLastWriteTime(assemblyCompound.File);

                        // using (var assemblyResolver = new AssemblyResolver(assemblyDirectory.AssemblyDirectory.AssemblyLoadContext, assemblyCompound.File))
                        // {
                        //     assemblyCompound.Assembly = assemblyResolver.Assembly;
                        //     assemblyCompound.Types = LoadTypes(assemblyCompound);
                        //     assemblyCompound.LastFileWriteTime = File.GetLastWriteTime(assemblyCompound.File);
                        // }
                        
                        // using (var fs = new FileStream(assemblyCompound.File, FileMode.Open))
                        // {
                        //     var assembly = assemblyDirectory.AssemblyDirectory.AssemblyLoadContext.LoadFromStream(fs);
                        //     assemblyCompound.Assembly = assembly;
                        //     assemblyCompound.Types = LoadTypes(assemblyCompound);
                        //     assemblyCompound.LastFileWriteTime = File.GetLastWriteTime(assemblyCompound.File);
                        // }

                        var cacheFile = UpdateCache(assemblyDirectory, assemblyCompound, cacheDirectory);

                        if (ShouldLoadAssembly(assemblyDirectory, assemblyCompound))
                        {
                            assemblyCompound.Assembly = Assembly.LoadFrom(cacheFile);
                            assemblyCompound.Types = LoadTypes(assemblyCompound);
                        }
                        
                        return ValueTask.CompletedTask;
                    }).ConfigureAwait(false);
                }
                else
                {
                    assemblyDirectory.AssemblyDirectory.AssemblyLoadContext = loadedAssemblyDirectory.AssemblyLoadContext;
                    assemblyDirectory.AssemblyDirectory.AssemblyCompounds = loadedAssemblyDirectory.AssemblyCompounds;
                }
            }
            else
            {
                Log.Debug("Load assemblies at {Path}", assemblyDirectory.AssemblyDirectory.AbsolutPath);

                assemblyDirectory.AssemblyDirectory.AssemblyLoadContext = new CustomAssemblyLoadContext(assemblyDirectory.AssemblyDirectory.AbsolutPath);

                await Parallel.ForEachAsync(assemblyDirectory.AssemblyDirectory.AssemblyCompounds, _, (assemblyCompound, _) =>
                {
                    Log.Debug("Load assembly at {Path}", assemblyCompound.File);

                    // var assembly = await _assemblyDependencyLoader
                    //     .LoadFromAssemblyPathAsync(assemblyDirectory, assemblyCompound.File)
                    //     .ConfigureAwait(false);
                    // assemblyCompound.Assembly = assembly;
                    // assemblyCompound.Types = LoadTypes(assemblyCompound);

                    // using (var assemblyResolver = new AssemblyResolver(assemblyDirectory.AssemblyDirectory.AssemblyLoadContext, assemblyCompound.File))
                    // {
                    //     assemblyCompound.Assembly = assemblyResolver.Assembly;
                    //     assemblyCompound.Types = LoadTypes(assemblyCompound);
                    // }
                    
                    // using (var fs = new FileStream(assemblyCompound.File, FileMode.Open))
                    // {
                    //     var assembly = assemblyDirectory.AssemblyDirectory.AssemblyLoadContext.LoadFromStream(fs);
                    //     assemblyCompound.Assembly = assembly;
                    //     assemblyCompound.Types = LoadTypes(assemblyCompound);
                    // }
                    
                    var cacheFile = UpdateCache(assemblyDirectory, assemblyCompound, cacheDirectory);

                    if (ShouldLoadAssembly(assemblyDirectory, assemblyCompound))
                    {
                        assemblyCompound.Assembly = Assembly.LoadFrom(cacheFile);
                        assemblyCompound.Types = LoadTypes(assemblyCompound);
                    }

                    return ValueTask.CompletedTask;
                }).ConfigureAwait(false);

                _assemblyDirectories.TryAdd(assemblyDirectory.AssemblyDirectory.AbsolutPath, assemblyDirectory.AssemblyDirectory);
            }
        }).ConfigureAwait(false);

        Log.Debug("Total of {0} assemblies have been loaded", CountAssemblies(configuration));
    }

    private static bool ShouldLoadAssembly(AssemblyDirectoryWithAssemblyRoot assemblyDirectory, AssemblyCompound assemblyCompound)
    {
        var fileName = Path.GetFileName(assemblyCompound.File);
        return assemblyDirectory.AssemblyRoot.IncludeAssemblyPattern.Any(pattern => pattern.IsMatch(fileName));
    }

    private static string UpdateCache(AssemblyDirectoryWithAssemblyRoot assemblyDirectory, AssemblyCompound assemblyCompound, string cacheDirectory)
    {
        var fileName = Path.GetFileName(assemblyCompound.File);
        var cacheDirectoryPath = Path.Combine(cacheDirectory, GetHashString(assemblyDirectory.AssemblyDirectory.AbsolutPath));
        Directory.CreateDirectory(cacheDirectoryPath);

        var cachedAssembly = Path.Combine(cacheDirectoryPath, fileName);

        if (AssemblyIsNewer(assemblyCompound.File, cachedAssembly))
        {
            File.Copy(assemblyCompound.File, cachedAssembly, true);
        }

        return cachedAssembly;
    }

    private static bool AnyAssemblyIsNewer(IReadOnlyCollection<AssemblyCompound> loadedAssemblyCompounds, List<AssemblyCompound> newAssemblyCompounds)
    {
        if (loadedAssemblyCompounds.Count != newAssemblyCompounds.Count)
        {
            return true;
        }

        var loadedCompoundDic = loadedAssemblyCompounds.ToDictionary(loadedCompound => loadedCompound.File, loadedCompound => loadedCompound);

        foreach (var newAssemblyCompound in newAssemblyCompounds)
        {
            if (!loadedCompoundDic.ContainsKey(newAssemblyCompound.File))
            {
                return true;
            }

            if (newAssemblyCompound.LastFileWriteTime > loadedCompoundDic[newAssemblyCompound.File].LastFileWriteTime)
            {
                return true;
            }
        }

        return false;
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