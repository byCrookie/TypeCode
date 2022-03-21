using System.Collections.Concurrent;
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
        var assemblyDirectories = configuration.AssemblyRoot
            .SelectMany(root => root.AssemblyGroup
                .SelectMany(group => group.AssemblyPath
                    .SelectMany(path => path.AssemblyDirectories)
                    .Concat(group.AssemblyPathSelector
                        .SelectMany(selector => selector.AssemblyDirectories))));

        await Parallel.ForEachAsync(assemblyDirectories, async (assemblyDirectory, _) =>
        {
            if (_assemblyDirectories.ContainsKey(assemblyDirectory.AbsolutPath))
            {
                var loadedAssemblyDirectory = _assemblyDirectories[assemblyDirectory.AbsolutPath];
                _assemblyDirectories.TryRemove(assemblyDirectory.AbsolutPath, out var _);
                _assemblyDirectories.TryAdd(assemblyDirectory.AbsolutPath, assemblyDirectory);

                if (AnyAssemblyIsNewer(loadedAssemblyDirectory.AssemblyCompounds, assemblyDirectory.AssemblyCompounds))
                {
                    Log.Debug("Reload assemblies at {Path}", assemblyDirectory.AbsolutPath);

                    loadedAssemblyDirectory.AssemblyLoadContext?.Unload();
                    assemblyDirectory.AssemblyLoadContext = new CustomAssemblyLoadContext(assemblyDirectory.AbsolutPath);

                    await Parallel.ForEachAsync(assemblyDirectory.AssemblyCompounds, _, async (assemblyCompound, _) =>
                    {
                        Log.Debug("Reload assembly at {Path}", assemblyCompound.File);
                        
                        var assembly = await _assemblyDependencyLoader
                            .LoadFromAssemblyPathAsync(assemblyDirectory.AssemblyLoadContext, assemblyCompound.File)
                            .ConfigureAwait(false);
                        
                        assemblyCompound.Assembly = assembly;
                        assemblyCompound.Types = LoadTypes(assemblyCompound);
                        assemblyCompound.LastFileWriteTime = File.GetLastWriteTime(assemblyCompound.File);
                    }).ConfigureAwait(false);
                }
                else
                {
                    assemblyDirectory.AssemblyLoadContext = loadedAssemblyDirectory.AssemblyLoadContext;
                    assemblyDirectory.AssemblyCompounds = loadedAssemblyDirectory.AssemblyCompounds;
                }
            }
            else
            {
                Log.Debug("Load assemblies at {Path}", assemblyDirectory.AbsolutPath);

                assemblyDirectory.AssemblyLoadContext = new CustomAssemblyLoadContext(assemblyDirectory.AbsolutPath);

                await Parallel.ForEachAsync(assemblyDirectory.AssemblyCompounds, _, async (assemblyCompound, _) =>
                {
                    Log.Debug("Load assembly at {Path}", assemblyCompound.File);

                    var assembly = await _assemblyDependencyLoader
                        .LoadFromAssemblyPathAsync(assemblyDirectory.AssemblyLoadContext, assemblyCompound.File)
                        .ConfigureAwait(false);
                    
                    assemblyCompound.Assembly = assembly;
                    assemblyCompound.Types = LoadTypes(assemblyCompound);
                }).ConfigureAwait(false);

                _assemblyDirectories.TryAdd(assemblyDirectory.AbsolutPath, assemblyDirectory);
            }
            
        }).ConfigureAwait(false);

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

            if (newAssemblyCompound.LastFileWriteTime >= loadedCompoundDic[newAssemblyCompound.File].LastFileWriteTime)
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