using System.Reflection;
using Serilog;
using TypeCode.Business.Configuration;

namespace TypeCode.Business.TypeEvaluation;

public class TypeEvaluator : ITypeEvaluator
{
    private static readonly object Lock = new();
    
    public TypeCodeConfiguration EvaluateTypes(TypeCodeConfiguration configuration)
    {
        try
        {
            var config = LoadTypes(configuration);
            Log.Debug("Types were loaded");
            return config;
        }
        catch (Exception ex)
        {
            throw new TypeLoadException("Loading types failed. " + ex.Message);
        }
    }

    private static TypeCodeConfiguration LoadTypes(TypeCodeConfiguration configuration)
    {
        Parallel.ForEach(configuration.AssemblyRoot, root =>
        {
            Parallel.ForEach(root.AssemblyGroup, group =>
            {
                Parallel.ForEach(group.AssemblyPath, path =>
                {
                    Parallel.ForEach(path.AssemblyDirectories, assemblyDirectory => { Parallel.ForEach(assemblyDirectory.Assemblies, assembly => Load(assembly, assemblyDirectory)); });
                });

                Parallel.ForEach(group.AssemblyPathSelector, selector =>
                {
                    Parallel.ForEach(selector.AssemblyDirectories, assemblyDirectory => { Parallel.ForEach(assemblyDirectory.Assemblies, assembly => Load(assembly, assemblyDirectory)); });
                });
            });
        });

        return configuration;
    }

    private static void Load(Assembly assembly, AssemblyDirectory assemblyDirectory)
    {
        var loadedTypes = assembly.GetLoadableTypes().ToList();
        Log.Debug("Loaded {Count} Types from {Assembly}", loadedTypes.Count, assembly.FullName);
        loadedTypes.ForEach(type => assemblyDirectory.Types.Add(type));

        lock (Lock)
        {
            File.AppendAllLines("TypeCode.LoadedTypes.txt", loadedTypes.Select(type => $"{type.FullName}.{type.AssemblyQualifiedName}"));
        }
    }
}