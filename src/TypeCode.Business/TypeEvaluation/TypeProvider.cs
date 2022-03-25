using Serilog;
using TypeCode.Business.Configuration;
using TypeCode.Business.Format;
using TypeCode.Business.Logging;

namespace TypeCode.Business.TypeEvaluation;

public class TypeProvider : ITypeProvider
{
    private static TypeCodeConfiguration? _configuration;
    private static readonly List<string> LoadedKeys = new();

    public async Task InitalizeAsync(TypeCodeConfiguration configuration)
    {
        Log.Debug("Initialize Types");

        foreach (var root in configuration.AssemblyRoot.OrderBy(root => root.Priority).ToList())
        {
            foreach (var group in root.AssemblyGroup.OrderBy(group => group.Priority).ToList())
            {
                foreach (var path in group.AssemblyPath.OrderBy(path => path.Priority).ToList())
                {
                    path.TypesByNameDictionary = path.AssemblyDirectories
                        .SelectMany(directory => directory.AssemblyCompounds.SelectMany(compund => compund.Types))
                        .GroupBy(GetNameWithoutGeneric)
                        .ToDictionary(nameGroup => nameGroup.Key, nameGroup => nameGroup.ToList());
                    
                    LoadedKeys.AddRange(path.TypesByNameDictionary.Keys);

                    path.TypesByFullNameDictionary = path.AssemblyDirectories
                        .SelectMany(directory => directory.AssemblyCompounds.SelectMany(compund => compund.Types))
                        .GroupBy(NameBuilder.GetNameWithNamespace)
                        .ToDictionary(nameGroup => nameGroup.Key, nameGroup => nameGroup.ToList());
                    
                    LoadedKeys.AddRange(path.TypesByFullNameDictionary.Keys);
                }

                foreach (var selector in group.AssemblyPathSelector.OrderBy(selector => selector.Priority).ToList())
                {
                    selector.TypesByNameDictionary = selector.AssemblyDirectories
                        .SelectMany(directory => directory.AssemblyCompounds.SelectMany(compund => compund.Types))
                        .GroupBy(GetNameWithoutGeneric)
                        .ToDictionary(nameGroup => nameGroup.Key, nameGroup => nameGroup.ToList());
                    
                    LoadedKeys.AddRange(selector.TypesByNameDictionary.Keys);

                    selector.TypesByFullNameDictionary = selector.AssemblyDirectories
                        .SelectMany(directory => directory.AssemblyCompounds.SelectMany(compund => compund.Types))
                        .GroupBy(NameBuilder.GetNameWithNamespace)
                        .ToDictionary(nameGroup => nameGroup.Key, nameGroup => nameGroup.ToList());
                    
                    LoadedKeys.AddRange(selector.TypesByFullNameDictionary.Keys);
                }
            }
        }
        
        await WriteKeysToFileAsync(LoadedKeys).ConfigureAwait(false);
        LoadedKeys.Clear();

        Log.Debug("Initialized Types");

        _configuration = configuration;
    }

    public bool HasByName(string? name)
    {
        return !string.IsNullOrEmpty(name) && GetTypesByName(name).Any();
    }

    public IEnumerable<Type> TryGetByName(string? name)
    {
        return !string.IsNullOrEmpty(name) ? GetTypesByName(name) : new List<Type>();
    }

    public IEnumerable<Type> TryGetByNames(IEnumerable<string> names)
    {
        return GetTypesByNames(names.ToList());
    }

    public IEnumerable<Type> TryGetTypesByCondition(Func<Type, bool> condition)
    {
        return GetTypesByCondition(condition);
    }

    private static List<Type> GetTypesFromDicByNames(IReadOnlyCollection<string> names, IDictionaryHolder dictionaryHolder, Func<IDictionaryHolder, IDictionary<string, List<Type>>> dictionary)
    {
        if (names.Any(name => dictionary(dictionaryHolder).ContainsKey(name)))
        {
            var types = new List<Type>();
            foreach (var name in names)
            {
                types.AddRange(dictionary(dictionaryHolder)[name]);
            }

            return types;
        }

        return new List<Type>();
    }

    private static List<Type> GetTypesFromDicByCondition(Func<Type, bool> condition, IDictionaryHolder dictionaryHolder, Func<IDictionaryHolder, IDictionary<string, List<Type>>> dictionary)
    {
        var typesLists = dictionary(dictionaryHolder).Values;
        return typesLists.SelectMany(types => types.Where(condition)).ToList();
    }

    private static List<Type> GetTypesFromDicByName(string name, IDictionaryHolder dictionaryHolder, Func<IDictionaryHolder, IDictionary<string, List<Type>>> dictionary)
    {
        return dictionary(dictionaryHolder).ContainsKey(name) ? dictionary(dictionaryHolder)[name] : new List<Type>();
    }

    private static string GetNameWithoutGeneric(Type type)
    {
        return type.Name.Contains('`') ? type.Name.Remove(type.Name.IndexOf("`", StringComparison.Ordinal), 2) : type.Name;
    }

    private static IEnumerable<Type> GetTypesByCondition(Func<Type, bool> condition)
    {
        if (_configuration is null)
        {
            throw new ArgumentNullException($"{typeof(TypeCodeConfiguration)} not yet set");
        }

        foreach (var root in _configuration.AssemblyRoot)
        {
            foreach (var group in root.AssemblyGroup)
            {
                foreach (var selector in group.AssemblyPathSelector)
                {
                    var types = GetTypesFromDicByCondition(condition, selector, holder => holder.TypesByNameDictionary);
                    if (types.Any())
                    {
                        return types;
                    }

                    types = GetTypesFromDicByCondition(condition, selector, holder => holder.TypesByFullNameDictionary);
                    if (types.Any())
                    {
                        return types;
                    }
                }

                foreach (var path in group.AssemblyPath)
                {
                    var types = GetTypesFromDicByCondition(condition, path, holder => holder.TypesByNameDictionary);
                    if (types.Any())
                    {
                        return types;
                    }

                    types = GetTypesFromDicByCondition(condition, path, holder => holder.TypesByFullNameDictionary);
                    if (types.Any())
                    {
                        return types;
                    }
                }
            }
        }

        return Enumerable.Empty<Type>();
    }

    private static IEnumerable<Type> GetTypesByNames(IReadOnlyCollection<string> names)
    {
        if (_configuration is null)
        {
            throw new ArgumentNullException($"{typeof(TypeCodeConfiguration)} not yet set");
        }

        foreach (var root in _configuration.AssemblyRoot)
        {
            foreach (var group in root.AssemblyGroup)
            {
                foreach (var selector in group.AssemblyPathSelector)
                {
                    var types = GetTypesFromDicByNames(names, selector, holder => holder.TypesByNameDictionary);
                    if (types.Any())
                    {
                        return types;
                    }

                    types = GetTypesFromDicByNames(names, selector, holder => holder.TypesByFullNameDictionary);
                    if (types.Any())
                    {
                        return types;
                    }
                }

                foreach (var path in group.AssemblyPath)
                {
                    var types = GetTypesFromDicByNames(names, path, holder => holder.TypesByNameDictionary);
                    if (types.Any())
                    {
                        return types;
                    }

                    types = GetTypesFromDicByNames(names, path, holder => holder.TypesByFullNameDictionary);
                    if (types.Any())
                    {
                        return types;
                    }
                }
            }
        }

        return Enumerable.Empty<Type>();
    }

    private static IEnumerable<Type> GetTypesByName(string name)
    {
        if (_configuration is null)
        {
            throw new ArgumentNullException($"{typeof(TypeCodeConfiguration)} not yet set");
        }

        foreach (var root in _configuration.AssemblyRoot)
        {
            foreach (var group in root.AssemblyGroup)
            {
                foreach (var selector in group.AssemblyPathSelector)
                {
                    var types = GetTypesFromDicByName(name, selector, holder => holder.TypesByNameDictionary);
                    if (types.Any())
                    {
                        return types;
                    }

                    types = GetTypesFromDicByName(name, selector, holder => holder.TypesByFullNameDictionary);
                    if (types.Any())
                    {
                        return types;
                    }
                }

                foreach (var path in group.AssemblyPath)
                {
                    var types = GetTypesFromDicByName(name, path, holder => holder.TypesByNameDictionary);
                    if (types.Any())
                    {
                        return types;
                    }

                    types = GetTypesFromDicByName(name, path, holder => holder.TypesByFullNameDictionary);
                    if (types.Any())
                    {
                        return types;
                    }
                }
            }
        }

        return Enumerable.Empty<Type>();
    }

    private static Task WriteKeysToFileAsync(IEnumerable<string> keys)
    {
        return File.AppendAllLinesAsync(LogFiles.IndexedTypes, keys);
    }
}