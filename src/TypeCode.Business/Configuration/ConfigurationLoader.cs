using System.Reflection;
using System.Text.RegularExpressions;
using Framework.Extensions.List;
using Serilog;
using TypeCode.Business.Format;

namespace TypeCode.Business.Configuration;

public class ConfigurationLoader : IConfigurationLoader
{
    private readonly IConfigurationMapper _configurationMapper;
    private readonly IGenericXmlSerializer _genericXmlSerializer;
    private readonly IAssemblyLoader _assemblyLoader;

    private IEnumerable<Regex> _includeFileRegexPatterns;

    public ConfigurationLoader(
        IConfigurationMapper configurationMapper,
        IGenericXmlSerializer genericXmlSerializer,
        IAssemblyLoader assemblyLoader
    )
    {
        _configurationMapper = configurationMapper;
        _genericXmlSerializer = genericXmlSerializer;
        _assemblyLoader = assemblyLoader;

        _includeFileRegexPatterns = new List<Regex>();
    }

    public async Task<TypeCodeConfiguration> LoadAsync()
    {
        Log.Debug("Evaluating .dll files");

        var xmlConfiguration = ReadXmlConfiguration();
        var configuration = _configurationMapper.MapToConfiguration(xmlConfiguration);

        await Parallel.ForEachAsync(configuration.AssemblyRoot, async
                (root, _) => await EvaluateAssemblyRootAsync(root).ConfigureAwait(false))
            .ConfigureAwait(false);

        Console.WriteLine($@"{Cuts.Short()} Assembly Priority");

        SetPriorties(configuration);

        await _assemblyLoader.LoadAsync(configuration).ConfigureAwait(false);
        
        return configuration;
    }

    private static void SetPriorties(TypeCodeConfiguration configuration)
    {
        foreach (var root in configuration.AssemblyRoot.OrderBy(r => r.Priority))
        {
            foreach (var group in root.AssemblyGroup)
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

                foreach (var message in messages.OrderBy(message => message.Priority).ToList())
                {
                    Log.Debug("{0}", message.Message);
                }

                group.PriorityAssemblyList = messages;
            }
        }
    }

    private Task EvaluateAssemblyRootAsync(AssemblyRoot root)
    {
        _includeFileRegexPatterns = root.IncludeAssemblyPattern
            .Select(pattern => new Regex(pattern, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase));

        return Parallel.ForEachAsync(root.AssemblyGroup, async (assemblyGroup, _) => await EvaluateAssemblyGroupAsync(root, assemblyGroup).ConfigureAwait(false));
    }

    private async Task EvaluateAssemblyGroupAsync(AssemblyRoot root, AssemblyGroup assemblyGroup)
    {
        await Parallel.ForEachAsync(assemblyGroup.AssemblyPathSelector, async
                (assemblyPathSelector, _) => await EvaluateAssemblyPathSelectorAsync(root, assemblyPathSelector).ConfigureAwait(false))
            .ConfigureAwait(false);

        await Parallel.ForEachAsync(assemblyGroup.AssemblyPath, async
                (assemblyPath, _) => await PrepareAssemblyDirectoriesAsync($@"{root.Path}{assemblyPath.Path}", assemblyPath).ConfigureAwait(false))
            .ConfigureAwait(false);
    }

    private Task EvaluateAssemblyPathSelectorAsync(AssemblyRoot root, AssemblyPathSelector assemblyPathSelector)
    {
        var directories = Directory.GetDirectories(root.Path)
            .Where(directory => Regex.IsMatch(directory, assemblyPathSelector.Selector))
            .Select(directory => $@"{directory}\{assemblyPathSelector.Path}");

        return Parallel.ForEachAsync(directories, async (directory, _) => await PrepareAssemblyDirectoriesAsync(directory, assemblyPathSelector).ConfigureAwait(false));
    }

    private Task PrepareAssemblyDirectoriesAsync(string absolutPath, IAssemblyHolder assemblyHolder)
    {
        if (Directory.Exists(absolutPath))
        {
            var assemblyDirectory = new AssemblyDirectory(
                assemblyHolder.Path,
                absolutPath
            );

            var filteredAssemblyFiles = Directory
                .GetFiles(absolutPath.Trim(), "*.dll")
                .Where(file => _includeFileRegexPatterns
                    .Any(pattern => pattern.IsMatch(file)))
                .ToList();

            assemblyDirectory.AssemblyCompounds = filteredAssemblyFiles
                .Select(file => new AssemblyCompound(file)).ToList();
            assemblyHolder.AssemblyDirectories.Add(assemblyDirectory);
        }

        return Task.CompletedTask;
    }

    private XmlTypeCodeConfiguration ReadXmlConfiguration()
    {
        var cfg = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\Configuration.cfg.xml";
        var xml = File.ReadAllText(cfg);
        return _genericXmlSerializer.Deserialize<XmlTypeCodeConfiguration>(xml) ?? throw new Exception($"{cfg} can not be parsed");
    }
}