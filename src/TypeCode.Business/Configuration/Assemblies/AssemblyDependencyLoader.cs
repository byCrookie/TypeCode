using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.Extensions.DependencyModel;
using Serilog;

namespace TypeCode.Business.Configuration.Assemblies;

public class AssemblyDependencyLoader : IAssemblyDependencyLoader
{
    private readonly ConcurrentDictionary<string, LoadLock> _locks = new();
    
    public async Task<Assembly?> LoadFromAssemblyPathAsync(AssemblyLoadContext assemblyLoadContext, string assemblyFullPath)
    {
        Log.Debug("Load {Assembly} to context {Context}", assemblyFullPath, assemblyLoadContext.Name);
        
        var fileNameWithOutExtension = Path.GetFileNameWithoutExtension(assemblyFullPath);
        var fileName = Path.GetFileName(assemblyFullPath);
        var directory = Path.GetDirectoryName(assemblyFullPath);

        if (directory is null)
        {
            return null;
        }

        var inCompileLibraries = DependencyContext.Default.CompileLibraries
            .Any(l => l.Name.Equals(fileNameWithOutExtension, StringComparison.OrdinalIgnoreCase));
        var inRuntimeLibraries = DependencyContext.Default.RuntimeLibraries
            .Any(l => l.Name.Equals(fileNameWithOutExtension, StringComparison.OrdinalIgnoreCase));

        var assembly = inCompileLibraries || inRuntimeLibraries
            ? assemblyLoadContext.LoadFromAssemblyName(new AssemblyName(fileNameWithOutExtension))
            : LoadAssembly(assemblyLoadContext, assemblyFullPath);

        await LoadReferencedAssembliesAsync(assemblyLoadContext, assembly, fileName, directory).ConfigureAwait(false);

        Log.Debug("Load of {Assembly} to context {Context} has finished", assemblyFullPath, assemblyLoadContext.Name);
        
        return assembly;
    }

    private async Task LoadReferencedAssembliesAsync(AssemblyLoadContext assemblyLoadContext, Assembly assembly, string fileName, string? directory)
    {
        Log.Debug("Load referenced assemblies from {Assembly} to context {Context}", assembly.FullName, assemblyLoadContext.Name);
        
        if (directory is null)
        {
            return;
        }

        var filesInDirectory = Directory.GetFiles(directory)
            .Where(x => x != fileName)
            .Select(Path.GetFileNameWithoutExtension)
            .ToList();

        var references = assembly.GetReferencedAssemblies();
        
        Log.Debug("Found {Count} referenced assemblies to load from {Assembly} to context {Context}", references.Length, assembly.FullName, assemblyLoadContext.Name);
        
        await Parallel.ForEachAsync(references, async (reference, _) =>
        {
            if (filesInDirectory.Contains(reference.Name))
            {
                var loadFileName = reference.Name + ".dll";
                var path = Path.Combine(directory, loadFileName);
                var loadedAssembly = LoadAssembly(assemblyLoadContext, path);
                await LoadReferencedAssembliesAsync(assemblyLoadContext, loadedAssembly, loadFileName, directory).ConfigureAwait(false);
            }
        }).ConfigureAwait(false);
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
}