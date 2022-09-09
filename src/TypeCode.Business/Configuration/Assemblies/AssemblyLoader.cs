using System.Collections.Concurrent;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Serilog;
using TypeCode.Business.Bootstrapping.Data;
using TypeCode.Business.TypeEvaluation;

namespace TypeCode.Business.Configuration.Assemblies;

public class AssemblyLoader : IAssemblyLoader
{
    private readonly IAssemblyDependencyLoader _assemblyDependencyLoader;
    private readonly IUserDataLocationProvider _userDataLocationProvider;
    private readonly ConcurrentDictionary<string, AssemblyDirectory> _assemblyDirectories = new();
    private readonly ConcurrentBag<string> _usedCacheDirectories = new();
    private const string CacheDirectoryPattern = "TypeCode_";

    public AssemblyLoader(IAssemblyDependencyLoader assemblyDependencyLoader, IUserDataLocationProvider userDataLocationProvider)
    {
        _assemblyDependencyLoader = assemblyDependencyLoader;
        _userDataLocationProvider = userDataLocationProvider;
    }

    public async Task LoadAsync(TypeCodeConfiguration configuration)
    {
        var assemblyRootCompounds = configuration.AssemblyRoot
            .Where(root => !root.Ignore)
            .SelectMany(root => root.AssemblyGroup.Where(group => !group.Ignore)
                .SelectMany(group => group.AssemblyPath.Where(path => !path.Ignore)
                    .SelectMany(path => path.AssemblyDirectories.Select(directory => new AssemblyRootCompound(root, directory)))
                    .Concat(group.AssemblyPathSelector.Where(selector => !selector.Ignore)
                        .SelectMany(selector => selector.AssemblyDirectories.Select(directory => new AssemblyRootCompound(root, directory))))))
            .ToList();

        await Parallel.ForEachAsync(assemblyRootCompounds, async (assemblyRootCompound, _) => { await Parallel.ForEachAsync(assemblyRootCompound.AssemblyDirectory.AssemblyCompounds, _, async (assemblyCompound, _) => { await CreateCacheAsync(assemblyCompound.File).ConfigureAwait(false); }).ConfigureAwait(false); }).ConfigureAwait(false);

        await Parallel.ForEachAsync(assemblyRootCompounds, async (assemblyRootCompound, _) =>
        {
            if (_assemblyDirectories.ContainsKey(assemblyRootCompound.AssemblyDirectory.AbsolutPath))
            {
                var loadedAssemblyDirectory = _assemblyDirectories[assemblyRootCompound.AssemblyDirectory.AbsolutPath];
                _assemblyDirectories.TryRemove(assemblyRootCompound.AssemblyDirectory.AbsolutPath, out var _);
                _assemblyDirectories.TryAdd(assemblyRootCompound.AssemblyDirectory.AbsolutPath, assemblyRootCompound.AssemblyDirectory);

                if (AnyAssemblyIsNewer(loadedAssemblyDirectory.AssemblyCompounds, assemblyRootCompound.AssemblyDirectory.AssemblyCompounds))
                {
                    Log.Debug("Reload assemblies at {Path}", assemblyRootCompound.AssemblyDirectory.AbsolutPath);
                    loadedAssemblyDirectory.AssemblyLoadContext?.Unload();
                    assemblyRootCompound.AssemblyDirectory.AssemblyLoadContext = new CustomAssemblyLoadContext(assemblyRootCompound.AssemblyDirectory.AbsolutPath);

                    await Parallel.ForEachAsync(assemblyRootCompound.AssemblyDirectory.AssemblyCompounds, _, async (assemblyCompound, _) =>
                    {
                        Log.Debug("Reload assembly at {Path}", assemblyCompound.File);

                        assemblyCompound.Assembly = await LoadAsync(assemblyRootCompound, assemblyCompound.File).ConfigureAwait(false);
                        assemblyCompound.Types = LoadTypes(assemblyCompound);
                        assemblyCompound.LastFileWriteTime = File.GetLastWriteTime(assemblyCompound.File);
                    }).ConfigureAwait(false);
                }
                else
                {
                    assemblyRootCompound.AssemblyDirectory.AssemblyLoadContext = loadedAssemblyDirectory.AssemblyLoadContext;
                    assemblyRootCompound.AssemblyDirectory.AssemblyCompounds = loadedAssemblyDirectory.AssemblyCompounds;
                }
            }
            else
            {
                Log.Debug("Load assemblies at {Path}", assemblyRootCompound.AssemblyDirectory.AbsolutPath);

                assemblyRootCompound.AssemblyDirectory.AssemblyLoadContext = new CustomAssemblyLoadContext(assemblyRootCompound.AssemblyDirectory.AbsolutPath);

                await Parallel.ForEachAsync(assemblyRootCompound.AssemblyDirectory.AssemblyCompounds, _, async (assemblyCompound, _) =>
                {
                    Log.Debug("Load assembly at {Path}", assemblyCompound.File);

                    assemblyCompound.Assembly = await LoadAsync(assemblyRootCompound, assemblyCompound.File).ConfigureAwait(false);
                    assemblyCompound.Types = LoadTypes(assemblyCompound);
                    assemblyCompound.LastFileWriteTime = File.GetLastWriteTime(assemblyCompound.File);
                }).ConfigureAwait(false);

                _assemblyDirectories.TryAdd(assemblyRootCompound.AssemblyDirectory.AbsolutPath, assemblyRootCompound.AssemblyDirectory);
            }
        }).ConfigureAwait(false);

        ClearUnusedCaches();

        Log.Debug("Total of {0} assemblies have been loaded", CountAssemblies(configuration));
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

    private void ClearUnusedCaches()
    {
        if (Directory.Exists(_userDataLocationProvider.GetCachePath()))
        {
            var caches = Directory.GetDirectories(_userDataLocationProvider.GetCachePath(), $"{CacheDirectoryPattern}*");
            foreach (var cache in caches)
            {
                if (!_usedCacheDirectories.Contains(cache) && Directory.Exists(cache))
                {
                    Directory.Delete(cache, true);
                }
            }
        }

        _usedCacheDirectories.Clear();
    }

    private Task CreateCacheAsync(string path)
    {
        try
        {
            var fileName = Path.GetFileNameWithoutExtension(path);
            var directoryName = Path.GetDirectoryName(path) ?? string.Empty;

            var cacheDirectoryPath = Path.Combine(_userDataLocationProvider.GetCachePath(), $"{CacheDirectoryPattern}{GetHashString(directoryName)}");
            Directory.CreateDirectory(cacheDirectoryPath);

            var cachedAssembly = Path.Combine(cacheDirectoryPath, $"{fileName}.dll");

            _usedCacheDirectories.Add(cacheDirectoryPath);

            if (!File.Exists(cachedAssembly) || AssemblyIsNewer(path, cachedAssembly))
            {
                File.Copy(path, cachedAssembly, true);
            }
        }
        catch (Exception exception)
        {
            Log.Warning("Error creating cache for {Assembly}: {Exception}", path, exception.Message);
        }

        return Task.CompletedTask;
    }

    private async Task<Assembly?> LoadAsync(AssemblyRootCompound assemblyRootCompound, string path)
    {
        try
        {
            var fileName = Path.GetFileName(path);
            var directoryName = Path.GetDirectoryName(path) ?? string.Empty;
            var cacheDirectoryPath = Path.Combine(_userDataLocationProvider.GetCachePath(), $"{CacheDirectoryPattern}{GetHashString(directoryName)}");
            var cachedAssembly = Path.Combine(cacheDirectoryPath, fileName);

            if (assemblyRootCompound.AssemblyRoot.IncludeAssemblyPattern.Any(pattern => pattern.IsMatch(fileName)))
            {
                return await _assemblyDependencyLoader.LoadWithDependenciesAsync(assemblyRootCompound, Path.GetFullPath(cachedAssembly)).ConfigureAwait(false);
            }

            return null;
        }
        catch (Exception exception)
        {
            Log.Warning("Error loading assembly {Assembly}: {Exception}", path, exception.Message);
            return null;
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