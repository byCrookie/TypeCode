using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.Loader;
using System.Text.RegularExpressions;
using Framework.Extensions.List;
using Serilog;
using TypeCode.Business.Bootstrapping.Data;
using TypeCode.Business.Format;
using TypeCode.Business.Logging;

namespace TypeCode.Business.TypeEvaluation;

public class DllTypeProvider : IDllTypeProvider
{
    private readonly IUserDataLocationProvider _userDataLocationProvider;
    private readonly ConcurrentDictionary<string, ConcurrentBag<Type>> _typesByNameDictionary;
    private readonly ConcurrentDictionary<string, ConcurrentBag<Type>> _typesByFullNameDictionary;

    public DllTypeProvider(IUserDataLocationProvider userDataLocationProvider)
    {
        _userDataLocationProvider = userDataLocationProvider;

        _typesByNameDictionary = new ConcurrentDictionary<string, ConcurrentBag<Type>>();
        _typesByFullNameDictionary = new ConcurrentDictionary<string, ConcurrentBag<Type>>();
    }

    public async Task InitalizeAsync(IEnumerable<string> targetDllPaths)
    {
        Log.Debug("Initialize Types");

        if (File.Exists(Path.Combine(_userDataLocationProvider.GetLogsPath(), LogFileNames.IndexedTypes)))
        {
            File.Delete(Path.Combine(_userDataLocationProvider.GetLogsPath(), LogFileNames.IndexedTypes));
        }

        var assemblyLoadContext = new AssemblyLoadContext("dll");
        var dllPaths = targetDllPaths.Where(Directory.Exists).SelectMany(path => Directory.GetFiles(path, "*.dll", SearchOption.TopDirectoryOnly));

        Parallel.ForEach(dllPaths, dllPath =>
        {
            using (var fs = new FileStream(dllPath, FileMode.Open, FileAccess.Read))
            {
                assemblyLoadContext.LoadFromStream(fs);
            }
        });

        var loadedKeys = new ConcurrentBag<string>();

        Parallel.ForEach(assemblyLoadContext.Assemblies, assembly =>
        {
            var types = LoadTypes(assembly);
            var typesByNameDictionary = types
                .GroupBy(NameBuilder.GetNameWithoutGeneric)
                .ToDictionary(nameGroup => nameGroup.Key, nameGroup => nameGroup.ToList());
            typesByNameDictionary.ForEach(entry => loadedKeys.Add(entry.Key));
            typesByNameDictionary.ForEach(entry => _typesByNameDictionary.AddOrUpdate(
                entry.Key,
                _ => new ConcurrentBag<Type>(entry.Value),
                (_, bag) =>
                {
                    entry.Value.ForEach(bag.Add);
                    return bag;
                })
            );

            var typesByFullNameDictionary = types
                .GroupBy(NameBuilder.GetNameWithNamespace)
                .ToDictionary(nameGroup => nameGroup.Key, nameGroup => nameGroup.ToList());
            typesByFullNameDictionary.ForEach(entry => loadedKeys.Add(entry.Key));
            typesByFullNameDictionary.ForEach(entry => _typesByFullNameDictionary.AddOrUpdate(
                entry.Key,
                _ => new ConcurrentBag<Type>(entry.Value),
                (_, bag) =>
                {
                    entry.Value.ForEach(bag.Add);
                    return bag;
                })
            );
        });


        await WriteKeysToFileAsync(loadedKeys).ConfigureAwait(false);
        loadedKeys.Clear();

        Log.Debug("Initialized Types");
    }

    public bool HasByName(string? name, TypeEvaluationOptions? options = null, CancellationToken? ct = null)
    {
        return TryGetByName(name, options).Any();
    }

    public IEnumerable<Type> TryGetByName(string? name, TypeEvaluationOptions? options = null, CancellationToken? ct = null)
    {
        if (string.IsNullOrEmpty(name))
        {
            return Enumerable.Empty<Type>();
        }

        if (options?.Regex ?? false)
        {
            return options.IgnoreCase
                ? GetTypes(dictionary =>
                    GetTypesFromDicByNameUsingRegex(
                        new Regex(name, RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant), dictionary))
                : GetTypes(dictionary =>
                    GetTypesFromDicByNameUsingRegex(new Regex(name, RegexOptions.Compiled | RegexOptions.CultureInvariant), dictionary));
        }

        return GetTypes(dictionary => GetTypesFromDicByName(name, dictionary));
    }

    public IEnumerable<Type> TryGetByNames(IReadOnlyList<string> names, TypeEvaluationOptions? options = null, CancellationToken? ct = null)
    {
        var types = new List<Type>();
        foreach (var name in names)
        {
            types.AddRange(TryGetByName(name, options));
        }

        return types;
    }

    public IEnumerable<Type> TryGetTypesByCondition(Func<Type, bool> condition, CancellationToken? ct = null)
    {
        return GetTypes(dictionary => GetTypesFromDicByCondition(condition, dictionary));
    }

    private IEnumerable<Type> GetTypes(Func<ConcurrentDictionary<string, ConcurrentBag<Type>>, List<Type>> evaluate)
    {
        var typesByName = evaluate(_typesByNameDictionary);
        return typesByName.Any() ? typesByName : evaluate(_typesByFullNameDictionary);
    }

    private static List<Type> GetTypesFromDicByName(string name, ConcurrentDictionary<string, ConcurrentBag<Type>> dictionary)
    {
        return dictionary.TryGetValue(name, out var value) ? value.ToList() : new List<Type>();
    }

    private static List<Type> GetTypesFromDicByNameUsingRegex(Regex regex, ConcurrentDictionary<string, ConcurrentBag<Type>> dictionary)
    {
        var matchingKeys = dictionary.Keys.Where(name => regex.IsMatch(name));

        var types = new List<Type>();

        foreach (var key in matchingKeys)
        {
            types.AddRange(dictionary[key]);
        }

        return types;
    }

    private static List<Type> GetTypesFromDicByCondition(Func<Type, bool> condition,
        ConcurrentDictionary<string, ConcurrentBag<Type>> dictionary)
    {
        var typesLists = dictionary.Values;
        return typesLists.SelectMany(types => types.Where(condition)).ToList();
    }

    private static List<Type> LoadTypes(Assembly assembly)
    {
        var loadedTypes = assembly.GetLoadableTypes().ToList();
        Log.Debug("Loaded {Count} Types from {Assembly}", loadedTypes.Count, assembly.FullName);
        return loadedTypes;
    }

    private Task WriteKeysToFileAsync(IEnumerable<string> keys)
    {
        return File.AppendAllLinesAsync(Path.Combine(_userDataLocationProvider.GetLogsPath(), LogFileNames.IndexedTypes), keys);
    }
}