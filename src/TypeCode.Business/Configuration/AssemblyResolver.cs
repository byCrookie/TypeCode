using System.Reflection;
using System.Runtime.Loader;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.DependencyModel.Resolution;

namespace TypeCode.Business.Configuration;

internal sealed class AssemblyResolver : IDisposable
{
    private readonly ICompilationAssemblyResolver _assemblyResolver;
    private readonly DependencyContext _dependencyContext;
    private readonly AssemblyLoadContext? _loadContext;

    public AssemblyResolver(AssemblyLoadContext assemblyLoadContext, string path)
    {
        Assembly = assemblyLoadContext.LoadFromAssemblyPath(path);
        _dependencyContext = DependencyContext.Load(Assembly);

        _assemblyResolver = new CompositeCompilationAssemblyResolver
                                (new ICompilationAssemblyResolver[]
        {
            new AppBaseCompilationAssemblyResolver(Path.GetDirectoryName(path)),
            new ReferenceAssemblyPathResolver(),
            new PackageCompilationAssemblyResolver()
        });

        _loadContext = assemblyLoadContext;

        if (_loadContext is not null)
        {
            _loadContext.Resolving += OnResolving;
        }
    }

    public Assembly Assembly { get; }

    public void Dispose()
    {
        if (_loadContext is not null)
        {
            _loadContext.Resolving -= OnResolving;
        }
    }

    private Assembly? OnResolving(AssemblyLoadContext context, AssemblyName name)
    {
        bool NamesMatch(RuntimeLibrary runtime)
        {
            return string.Equals(runtime.Name, name.Name, StringComparison.OrdinalIgnoreCase);
        }

        var library = _dependencyContext.RuntimeLibraries.FirstOrDefault(NamesMatch);
        
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
            if (assemblies.Count > 0 && _loadContext is not null)
            {
                return _loadContext.LoadFromAssemblyPath(assemblies[0]);
            }
        }

        return null;
    }
}