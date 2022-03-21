using System.Reflection;
using System.Runtime.Loader;
using Microsoft.Extensions.DependencyModel;

namespace TypeCode.Business.Configuration.Assemblies;

public class AssemblyDependencyLoader : IAssemblyDependencyLoader
{
    public async Task<Assembly?> LoadFromAssemblyPathAsync(AssemblyLoadContext assemblyLoadContext, string assemblyFullPath)
    {
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

        return assembly;
    }

    private static async Task LoadReferencedAssembliesAsync(AssemblyLoadContext assemblyLoadContext, Assembly assembly, string fileName, string? directory)
    {
        if (directory is null)
        {
            return;
        }

        var filesInDirectory = Directory.GetFiles(directory)
            .Where(x => x != fileName)
            .Select(Path.GetFileNameWithoutExtension)
            .ToList();

        var references = assembly.GetReferencedAssemblies();

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

    private static Assembly LoadAssembly(AssemblyLoadContext assemblyLoadContext, string assemblyFullPath)
    {
        using (var fs = new FileStream(assemblyFullPath, FileMode.Open))
        {
            return assemblyLoadContext.LoadFromStream(fs);
        }
    }
}