using System.Reflection;
using System.Runtime.Loader;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.DependencyModel.Resolution;

namespace TypeCode.Business.Configuration.Assemblies;

internal sealed class AssemblyResolver : IDisposable
{
    private readonly AssemblyLoadContext _assemblyLoadContext;
    private readonly DependencyContext _dependencyContext;
    private readonly ICompilationAssemblyResolver _assemblyResolver;

    public AssemblyResolver(AssemblyLoadContext assemblyLoadContext, string path)
    {
        _assemblyLoadContext = assemblyLoadContext;
        
        // using (var fs = new FileStream(path, FileMode.Open))
        // {
        //     Assembly = _assemblyLoadContext.LoadFromStream(fs);
        // }
        
        Assembly = assemblyLoadContext.LoadFromAssemblyPath(path);

        _dependencyContext = DependencyContext.Load(Assembly);
        _assemblyResolver = new CompositeCompilationAssemblyResolver(new ICompilationAssemblyResolver[]
        {
            new AppBaseCompilationAssemblyResolver(Path.GetDirectoryName(path)),
            new ReferenceAssemblyPathResolver(),
            new PackageCompilationAssemblyResolver()
        });

        _assemblyLoadContext.Resolving += OnResolving;
    }

    public Assembly Assembly { get; }

    public void Dispose()
    {
        _assemblyLoadContext.Resolving -=  OnResolving;
    }

    private Assembly? OnResolving(AssemblyLoadContext loadContext, AssemblyName name)
    {
        var library = _dependencyContext.RuntimeLibraries
            .FirstOrDefault(runtimeLibrary => NamesMatch(runtimeLibrary, name));

        if (library != null)
        {
            var wrapper = new CompilationLibrary(
                library.Type,
                library.Name,
                library.Version,
                library.Hash,
                library.RuntimeAssemblyGroups.SelectMany(g => g.AssetPaths),
                library.Dependencies,
                library.Serviceable);

            var assemblies = new List<string>();
            _assemblyResolver.TryResolveAssemblyPaths(wrapper, assemblies);
            
            if (assemblies.Count > 0)
            {
                // using (var fs = new FileStream(assemblies[0], FileMode.Open))
                // {
                //     return _assemblyLoadContext.LoadFromStream(fs);
                // }
                
                return _assemblyLoadContext.LoadFromAssemblyPath(assemblies[0]);
            }
        }

        return null;
    }
    
    private static bool NamesMatch(Library runtime, AssemblyName assemblyName)
    {
        return string.Equals(runtime.Name, assemblyName.Name, StringComparison.OrdinalIgnoreCase);
    }
}