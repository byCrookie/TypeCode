using System.Text.RegularExpressions;
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

        if (File.Exists(LogFilePaths.IndexedTypes))
        {
            File.Delete(LogFilePaths.IndexedTypes);
        }

        foreach (var root in configuration.AssemblyRoot.OrderBy(root => root.Priority).ToList())
        {
            foreach (var group in root.AssemblyGroup.OrderBy(group => group.Priority).ToList())
            {
                foreach (var path in group.AssemblyPath.OrderBy(path => path.Priority).ToList())
                {
                    path.TypesByNameDictionary = path.AssemblyDirectories
                        .SelectMany(directory => directory.AssemblyCompounds.SelectMany(compund => compund.Types))
                        .GroupBy(NameBuilder.GetNameWithoutGeneric)
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
                        .GroupBy(NameBuilder.GetNameWithoutGeneric)
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

    public bool HasByName(string? name, TypeEvaluationOptions? options = null)
    {
        return TryGetByName(name, options).Any();
    }

    public IEnumerable<Type> TryGetByName(string? name, TypeEvaluationOptions? options = null)
    {
        if (!string.IsNullOrEmpty(name))
        {
            if (options?.Regex ?? false)
            {
                return GetTypes(dictionary => GetTypesFromDicByNameUsingRegex(new Regex(name, RegexOptions.Compiled), dictionary));
            }

            return GetTypes(dictionary => GetTypesFromDicByName(name, dictionary));
        }

        return Enumerable.Empty<Type>();
    }

    public IEnumerable<Type> TryGetByNames(IReadOnlyList<string> names, TypeEvaluationOptions? options = null)
    {
        var types = new List<Type>();
        foreach (var name in names)
        {
            types.AddRange(TryGetByName(name, options));
        }
        return types;
    }

    public IEnumerable<Type> TryGetTypesByCondition(Func<Type, bool> condition)
    {
        return GetTypes(dictionary => GetTypesFromDicByCondition(condition, dictionary));
    }

    private static List<Type> GetTypesFromDicByName(string name, IDictionary<string, List<Type>> dictionary)
    {
        return dictionary.ContainsKey(name) ? dictionary[name] : new List<Type>();
    }

    private static List<Type> GetTypesFromDicByNameUsingRegex(Regex regex, IDictionary<string, List<Type>> dictionary)
    {
        var matchingKeys = dictionary.Keys.Where(name => regex.IsMatch(name));

        var types = new List<Type>();

        foreach (var key in matchingKeys)
        {
            types.AddRange(dictionary[key]);
        }

        return types;
    }

    private static List<Type> GetTypesFromDicByCondition(Func<Type, bool> condition, IDictionary<string, List<Type>> dictionary)
    {
        var typesLists = dictionary.Values;
        return typesLists.SelectMany(types => types.Where(condition)).ToList();
    }

    private static IEnumerable<Type> GetTypes(Func<IDictionary<string, List<Type>>, List<Type>> evaluate)
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
                    var types = evaluate(selector.TypesByNameDictionary);
                    if (types.Any())
                    {
                        return types;
                    }

                    types = evaluate(selector.TypesByFullNameDictionary);
                    if (types.Any())
                    {
                        return types;
                    }
                }

                foreach (var path in group.AssemblyPath)
                {
                    var types = evaluate(path.TypesByNameDictionary);
                    if (types.Any())
                    {
                        return types;
                    }

                    types = evaluate(path.TypesByFullNameDictionary);
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
        return File.AppendAllLinesAsync(LogFilePaths.IndexedTypes, keys);
    }
}