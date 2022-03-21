using System.Reflection;
using System.Runtime.Loader;
using Microsoft.Extensions.DependencyModel;

namespace TypeCode.Business.Configuration.Assemblies;

public class AssemblyDependencyLoader : IAssemblyDependencyLoader
{
    public Assembly? LoadFromAssemblyPath(AssemblyLoadContext assemblyLoadContext, string assemblyFullPath)
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

        LoadReferencedAssemblies(assemblyLoadContext, assembly, fileName, directory);

        return assembly;
    }

    private static void LoadReferencedAssemblies(AssemblyLoadContext assemblyLoadContext, Assembly assembly, string fileName, string? directory)
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

        foreach (var reference in references)
        {
            if (filesInDirectory.Contains(reference.Name))
            {
                var loadFileName = reference.Name + ".dll";
                var path = Path.Combine(directory, loadFileName);
                var loadedAssembly = LoadAssembly(assemblyLoadContext, path);
                LoadReferencedAssemblies(assemblyLoadContext, loadedAssembly, loadFileName, directory);
            }
        }
    }

    private static Assembly LoadAssembly(AssemblyLoadContext assemblyLoadContext, string assemblyFullPath)
    {
        using (var fs = new FileStream(assemblyFullPath, FileMode.Open))
        {
            return assemblyLoadContext.LoadFromStream(fs);
        }
    }
}