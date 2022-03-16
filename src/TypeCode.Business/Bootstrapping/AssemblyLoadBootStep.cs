using System.Reflection;
using System.Text.RegularExpressions;
using Framework.Autofac.Boot;
using Framework.Extensions.List;
using Serilog;
using TypeCode.Business.Configuration;
using TypeCode.Business.Format;
using Workflow;

namespace TypeCode.Business.Bootstrapping;

internal class AssemblyLoadBootStep<TContext> : IAssemblyLoadBootStep<TContext>
    where TContext : WorkflowBaseContext, IBootContext
{
    private readonly IConfigurationMapper _configurationMapper;
    private readonly IGenericXmlSerializer _genericXmlSerializer;
    private readonly IAssemblyLoader _assemblyLoader;

    private IEnumerable<Regex> _includeFileRegexPatterns;

    public AssemblyLoadBootStep(IConfigurationMapper configurationMapper, IGenericXmlSerializer genericXmlSerializer, IAssemblyLoader assemblyLoader)
    {
        _configurationMapper = configurationMapper;
        _genericXmlSerializer = genericXmlSerializer;
        _assemblyLoader = assemblyLoader;

        _includeFileRegexPatterns = new List<Regex>();
    }

    public async Task ExecuteAsync(TContext context)
    {
        Log.Debug("Evaluating .dll files");

        var xmlConfiguration = ReadXmlConfiguration();
        var configuration = _configurationMapper.MapToConfiguration(xmlConfiguration);

        await Parallel.ForEachAsync(configuration.AssemblyRoot, async
          (root, _) => await EvaluateAssemblyRootAsync(root).ConfigureAwait(false))
      .ConfigureAwait(false);

        Console.WriteLine($@"{Cuts.Short()} Assembly Priority");

        TraverseAssemblyRoots(configuration);

        Log.Debug("Total of {0} assemblies have been loaded", CountAssemblies(configuration));
        var assemblyProvider = new ConfigurationProvider();
        assemblyProvider.SetConfiguration(configuration);
        AssemblyLoadProvider.SetAssemblyProvider(assemblyProvider);
    }
    
    public Task<bool> ShouldExecuteAsync(TContext context)
    {
        return context.ShouldExecuteAsync();
    }

    private static void TraverseAssemblyRoots(TypeCodeConfiguration configuration)
    {
        foreach (var root in configuration.AssemblyRoot.OrderBy(r => r.Priority))
        {
            TraverseAssemblyGroups(root);
        }
    }

    private static void TraverseAssemblyGroups(AssemblyRoot root)
    {
        foreach (var group in root.AssemblyGroup)
        {
            var messages = new List<PriorityString>();

            @group.AssemblyPathSelector.ForEach(selector =>
            {
                selector.AssemblyDirectories
                    .ForEach(directory => messages
                        .Add(new PriorityString($"{root.Priority}.{@group.Priority}.{selector.Priority}",
                            $@"{Cuts.Point()} {directory.AbsolutPath}")));
            });

            @group.AssemblyPath.ForEach(path =>
            {
                path.AssemblyDirectories
                    .ForEach(directory => messages
                        .Add(new PriorityString($"{root.Priority}.{@group.Priority}.{path.Priority}",
                            $@"{directory.AbsolutPath}")));
            });

            WriteMessagesToConsole(messages);

            @group.PriorityAssemblyList = messages;
        }
    }

    private static void WriteMessagesToConsole(List<PriorityString> messages)
    {
        foreach (var message in messages.OrderBy(message => message.Priority).ToList())
        {
            Console.WriteLine(message.Message);
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
            (assemblyPath,_) => await LoadAssembliesAsync($@"{root.Path}{assemblyPath.Path}", assemblyPath).ConfigureAwait(false))
            .ConfigureAwait(false);
    }

    private Task EvaluateAssemblyPathSelectorAsync(AssemblyRoot root, AssemblyPathSelector assemblyPathSelector)
    {
        var directories = Directory.GetDirectories(root.Path)
            .Where(directory => Regex.IsMatch(directory, assemblyPathSelector.Selector))
            .Select(directory => $@"{directory}\{assemblyPathSelector.Path}");

        return Parallel.ForEachAsync(directories, async (directory, _) => await LoadAssembliesAsync(directory, assemblyPathSelector).ConfigureAwait(false));
    }

    private static int CountAssemblies(TypeCodeConfiguration configuration)
    {
        return configuration.AssemblyRoot
                   .Sum(root => root.AssemblyGroup
                       .Sum(group => group.AssemblyPath
                           .Sum(path => path.AssemblyDirectories
                               .Sum(directory => directory.Assemblies.Count))))
               +
               configuration.AssemblyRoot
                   .Sum(root => root.AssemblyGroup
                       .Sum(group => group.AssemblyPathSelector
                           .Sum(path => path.AssemblyDirectories
                               .Sum(directory => directory.Assemblies.Count))));
    }

    private async Task LoadAssembliesAsync(string absolutPath, IAssemblyHolder assemblyHolder)
    {
        if (Directory.Exists(absolutPath))
        {            
            var assemblyDirectory = new AssemblyDirectory(assemblyHolder.Path, absolutPath, Directory.GetFiles(absolutPath.Trim(), "*.dll"));

            var filteredFiles = assemblyDirectory.Files
                .Where(file => _includeFileRegexPatterns
                    .Any(pattern => pattern.IsMatch(file)))
                .ToList();

            Log.Debug("Loading {0} assemblies", filteredFiles.Count);

            await Parallel.ForEachAsync(filteredFiles, async (file, _) =>
            {
              try
              {
                var assembly = await _assemblyLoader.LoadAsync(file).ConfigureAwait(false);
                assemblyDirectory.Assemblies.Add(assembly);
              }
              catch (Exception e)
              {
                Log.Warning("{0}: {1}", Path.GetFileName(file), e.Message);
              }
            });

            Log.Debug("Loaded {0} assemblies", assemblyDirectory.Assemblies.Count);

            assemblyHolder.AssemblyDirectories.Add(assemblyDirectory);
        }
    }

    private XmlTypeCodeConfiguration ReadXmlConfiguration()
    {
        var cfg = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\Configuration.cfg.xml";
        var xml = File.ReadAllText(cfg);
        return _genericXmlSerializer.Deserialize<XmlTypeCodeConfiguration>(xml) ?? throw new Exception($"{cfg} can not be parsed");
    }
}