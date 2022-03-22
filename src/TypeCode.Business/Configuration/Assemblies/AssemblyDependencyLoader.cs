using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.Extensions.DependencyModel;
using Serilog;

namespace TypeCode.Business.Configuration.Assemblies;

public class AssemblyDependencyLoader : IAssemblyDependencyLoader
{
    private readonly ConcurrentDictionary<string, LoadLock> _locks = new();

    public async Task<Assembly?> LoadFromAssemblyPathAsync(AssemblyDirectoryWithAssemblyRoot assemblyDirectory, string assemblyFullPath)
    {
        Log.Debug("Load {Assembly} to context {Context}", assemblyFullPath, assemblyDirectory.AssemblyDirectory.AssemblyLoadContext!.Name);

        var fileNameWithOutExtension = Path.GetFileNameWithoutExtension(assemblyFullPath);
        var fileName = Path.GetFileName(assemblyFullPath);
        var directory = Path.GetDirectoryName(assemblyFullPath);

        var assembly = LoadFromLoadedOrFile(assemblyDirectory, assemblyFullPath, fileNameWithOutExtension);

        await LoadReferencedAssembliesAsync(assemblyDirectory, assembly, fileName, directory).ConfigureAwait(false);

        Log.Debug("Load of {Assembly} to context {Context} has finished", assemblyFullPath, assemblyDirectory.AssemblyDirectory.AssemblyLoadContext.Name);

        return assembly;
    }

    private Task LoadReferencedAssembliesAsync(AssemblyDirectoryWithAssemblyRoot assemblyDirectory, Assembly assembly, string fileName, string? directory)
    {
        Log.Debug("Load referenced assemblies from {Assembly} to context {Context}", assembly.FullName, assemblyDirectory.AssemblyDirectory.AssemblyLoadContext!.Name);

        var filesInDirectory = Directory.GetFiles(directory!)
            .Where(x => x != fileName)
            .Select(Path.GetFileName)
            .Where(fileInDirectoryName =>
                fileInDirectoryName is not null
                && assemblyDirectory.AssemblyRoot.IncludeAssemblyPattern.Any(pattern => pattern.IsMatch(fileInDirectoryName)))
            .Select(Path.GetFileNameWithoutExtension)
            .ToList();

        var references = assembly.GetReferencedAssemblies();

        Log.Debug("Found {Count} referenced assemblies to load from {Assembly} to context {Context}", references.Length, assembly.FullName, assemblyDirectory.AssemblyDirectory.AssemblyLoadContext.Name);

        return Parallel.ForEachAsync(references, async (reference, _) =>
        {
            if (filesInDirectory.Contains(reference.Name))
            {
                var loadFileName = reference.Name + ".dll";
                var path = Path.Combine(directory!, loadFileName);
                var loadedAssembly = LoadFromLoadedOrFile(assemblyDirectory, path, reference.Name!);
                await LoadReferencedAssembliesAsync(assemblyDirectory, loadedAssembly, loadFileName, directory).ConfigureAwait(false);
            }
        });
    }

    private Assembly LoadAssembly(AssemblyLoadContext assemblyLoadContext, string assemblyFullPath)
    {
        lock (_locks.GetOrAdd(assemblyFullPath, _ => new LoadLock()))
        {
            using (var fs = new FileStream(assemblyFullPath, FileMode.Open))
            {
                return assemblyLoadContext.LoadFromStream(fs);
            }
        }
    }
    
    private Assembly LoadFromLoadedOrFile(AssemblyDirectoryWithAssemblyRoot assemblyDirectory, string assemblyFullPath, string fileNameWithOutExtension)
    {
        var inCompileLibraries = DependencyContext.Default.CompileLibraries
            .Any(l => l.Name.Equals(fileNameWithOutExtension, StringComparison.OrdinalIgnoreCase));
        var inRuntimeLibraries = DependencyContext.Default.RuntimeLibraries
            .Any(l => l.Name.Equals(fileNameWithOutExtension, StringComparison.OrdinalIgnoreCase));

        var assembly = inCompileLibraries || inRuntimeLibraries
            ? assemblyDirectory.AssemblyDirectory.AssemblyLoadContext!.LoadFromAssemblyName(new AssemblyName(fileNameWithOutExtension))
            : LoadAssembly(assemblyDirectory.AssemblyDirectory.AssemblyLoadContext!, assemblyFullPath);

        return assembly;
    }
}