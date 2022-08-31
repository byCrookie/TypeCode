using System.Text.RegularExpressions;
using Serilog;
using TypeCode.Business.Configuration.Assemblies;
using TypeCode.Business.Configuration.Location;
using TypeCode.Business.Format;

namespace TypeCode.Business.Configuration;

public class ConfigurationLoader : IConfigurationLoader
{
    private readonly IConfigurationMapper _configurationMapper;
    private readonly IGenericXmlSerializer _genericXmlSerializer;
    private readonly IAssemblyLoader _assemblyLoader;
    private readonly IConfigurationLocationProvider _configurationLocationProvider;

    public ConfigurationLoader(
        IConfigurationMapper configurationMapper,
        IGenericXmlSerializer genericXmlSerializer,
        IAssemblyLoader assemblyLoader,
        IConfigurationLocationProvider configurationLocationProvider
    )
    {
        _configurationMapper = configurationMapper;
        _genericXmlSerializer = genericXmlSerializer;
        _assemblyLoader = assemblyLoader;
        _configurationLocationProvider = configurationLocationProvider;
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
                var messages = new List<PriorityString>();

                group.AssemblyPathSelector.ForEach(selector =>
                {
                    selector.AssemblyDirectories
                        .ForEach(directory => messages
                            .Add(new PriorityString($"{root.Priority}.{group.Priority}.{selector.Priority}",
                                $@"{Cuts.Point()} {directory.AbsolutPath}")));
                });

                group.AssemblyPath.ForEach(path =>
                {
                    path.AssemblyDirectories
                        .ForEach(directory => messages
                            .Add(new PriorityString($"{root.Priority}.{group.Priority}.{path.Priority}",
                                $@"{directory.AbsolutPath}")));
                });

                foreach (var message in messages.OrderBy(message => message.Priority))
                {
                    Log.Information("{0}", message.Message);
                }

                group.PriorityAssemblyList = messages;
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
        var cfg = await _configurationLocationProvider.GetOrCreateAsync().ConfigureAwait(false);
        var xml = await File.ReadAllTextAsync(cfg).ConfigureAwait(false);
        return _genericXmlSerializer.Deserialize<XmlTypeCodeConfiguration>(xml) ?? throw new Exception($"{cfg} can not be parsed");
    }
}