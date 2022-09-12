using System.Text.RegularExpressions;
using Serilog;
using TypeCode.Business.Bootstrapping.Data;
using TypeCode.Business.Configuration.Assemblies;
using TypeCode.Business.Format;

namespace TypeCode.Business.Configuration;

public sealed class ConfigurationLoader : IConfigurationLoader
{
    private readonly IConfigurationMapper _configurationMapper;
    private readonly IGenericXmlSerializer _genericXmlSerializer;
    private readonly IAssemblyLoader _assemblyLoader;
    private readonly IUserDataLocationProvider _userDataLocationProvider;

    public ConfigurationLoader(
        IConfigurationMapper configurationMapper,
        IGenericXmlSerializer genericXmlSerializer,
        IAssemblyLoader assemblyLoader,
        IUserDataLocationProvider userDataLocationProvider
    )
    {
        _configurationMapper = configurationMapper;
        _genericXmlSerializer = genericXmlSerializer;
        _assemblyLoader = assemblyLoader;
        _userDataLocationProvider = userDataLocationProvider;
    }

    public async Task<TypeCodeConfiguration> LoadAsync()
    {
        Log.Debug("Evaluating .dll files");

        var xmlConfiguration = await ReadXmlConfigurationAsync().ConfigureAwait(false);
        var configuration = _configurationMapper.MapToConfiguration(xmlConfiguration);

        await Parallel.ForEachAsync(configuration.AssemblyRoot, async
                (root, _) => await EvaluateAssemblyRootAsync(root).ConfigureAwait(false))
            .ConfigureAwait(false);

        Log.Information("{Cut} Assembly Priority", Cuts.Short());

        SetPriorties(configuration);

        await _assemblyLoader.LoadAsync(configuration).ConfigureAwait(false);

        return configuration;
    }

    private static void SetPriorties(TypeCodeConfiguration configuration)
    {
        foreach (var root in configuration.AssemblyRoot.OrderBy(r => r.Priority))
        {
            foreach (var group in root.AssemblyGroup.OrderBy(r => r.Priority))
            {
                var assemblyPriorityStrings = new List<AssemblyPriorityString>();

                group.AssemblyPathSelector.ForEach(selector =>
                {
                    selector.AssemblyDirectories
                        .ForEach(directory => assemblyPriorityStrings
                            .Add(new AssemblyPriorityString($"{root.Priority}.{group.Priority}.{selector.Priority}",
                                directory.AbsolutPath, selector.Ignore || group.Ignore || root.Ignore)));
                });

                group.AssemblyPath.ForEach(path =>
                {
                    path.AssemblyDirectories
                        .ForEach(directory => assemblyPriorityStrings
                            .Add(new AssemblyPriorityString($"{root.Priority}.{group.Priority}.{path.Priority}",
                                directory.AbsolutPath, path.Ignore || group.Ignore || root.Ignore)));
                });

                foreach (var assemblyPriorityString in assemblyPriorityStrings.OrderBy(priorityString => priorityString.Priority))
                {
                    Log.Information("{0}", $"{assemblyPriorityString}");
                }

                group.PriorityAssemblyList = assemblyPriorityStrings;
            }
        }
    }

    private static Task EvaluateAssemblyRootAsync(AssemblyRoot root)
    {
        return Parallel.ForEachAsync(root.AssemblyGroup, async (assemblyGroup, _) => await EvaluateAssemblyGroupAsync(root, assemblyGroup).ConfigureAwait(false));
    }

    private static async Task EvaluateAssemblyGroupAsync(AssemblyRoot root, AssemblyGroup assemblyGroup)
    {
        await Parallel.ForEachAsync(assemblyGroup.AssemblyPathSelector, async
                (assemblyPathSelector, _) => await EvaluateAssemblyPathSelectorAsync(root, assemblyPathSelector).ConfigureAwait(false))
            .ConfigureAwait(false);

        await Parallel.ForEachAsync(assemblyGroup.AssemblyPath, async
                (assemblyPath, _) => await PrepareAssemblyDirectoriesAsync($@"{root.Path}{assemblyPath.Path}", assemblyPath).ConfigureAwait(false))
            .ConfigureAwait(false);
    }

    private static Task EvaluateAssemblyPathSelectorAsync(AssemblyRoot root, AssemblyPathSelector assemblyPathSelector)
    {
        if (!Directory.Exists(root.Path))
        {
            return Task.CompletedTask;
        }

        var directories = Directory.GetDirectories(root.Path)
            .Where(directory => Regex.IsMatch(directory, assemblyPathSelector.Selector))
            .Select(directory => $@"{directory}\{assemblyPathSelector.Path}");

        return Parallel.ForEachAsync(directories, async (directory, _) => await PrepareAssemblyDirectoriesAsync(directory, assemblyPathSelector).ConfigureAwait(false));
    }

    private static Task PrepareAssemblyDirectoriesAsync(string absolutPath, IAssemblyHolder assemblyHolder)
    {
        if (!Directory.Exists(absolutPath))
        {
            return Task.CompletedTask;
        }

        var assemblyDirectory = new AssemblyDirectory(
            assemblyHolder.Path,
            absolutPath
        );

        var files = Directory.GetFiles(absolutPath.Trim(), "*.dll")
            .Select(file => new { FileName = Path.GetFileName(file), Path = file });

        assemblyDirectory.AssemblyCompounds = files
            .Select(file => new AssemblyCompound(file.Path)).ToList();
        assemblyHolder.AssemblyDirectories.Add(assemblyDirectory);

        return Task.CompletedTask;
    }

    private async Task<XmlTypeCodeConfiguration> ReadXmlConfigurationAsync()
    {
        var cfg = _userDataLocationProvider.GetConfigurationFilePath();
        var xml = await File.ReadAllTextAsync(cfg).ConfigureAwait(false);
        return _genericXmlSerializer.Deserialize<XmlTypeCodeConfiguration>(xml) ?? throw new Exception($"{cfg} can not be parsed");
    }
}