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

    private IEnumerable<Regex> _includeFileRegexPatterns;

    public AssemblyLoadBootStep(IConfigurationMapper configurationMapper, IGenericXmlSerializer genericXmlSerializer)
    {
        _configurationMapper = configurationMapper;
        _genericXmlSerializer = genericXmlSerializer;

        _includeFileRegexPatterns = new List<Regex>();
    }

    public Task ExecuteAsync(TContext context)
    {
        Log.Debug("Evaluating .dll files");

        var xmlConfiguration = ReadXmlConfiguration();
        var configuration = _configurationMapper.MapToConfiguration(xmlConfiguration);

        Parallel.ForEach(configuration.AssemblyRoot, EvaluateAssemblyRoot);

        Console.WriteLine($@"{Cuts.Short()} Assembly Priority");

        TraverseAssemblyRoots(configuration);

        Log.Debug("Total of {0} assemblies have been loaded", CountAssemblies(configuration));
        var assemblyProvider = new ConfigurationProvider();
        assemblyProvider.SetConfiguration(configuration);
        AssemblyLoadProvider.SetAssemblyProvider(assemblyProvider);
        return Task.CompletedTask;
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

    private void EvaluateAssemblyRoot(AssemblyRoot root)
    {
        _includeFileRegexPatterns = root.IncludeAssemblyPattern
            .Select(pattern => new Regex(pattern, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase));

        Parallel.ForEach(root.AssemblyGroup, assemblyGroup => EvaluateAssemblyGroup(root, assemblyGroup));
    }

    private void EvaluateAssemblyGroup(AssemblyRoot root, AssemblyGroup assemblyGroup)
    {
        Parallel.ForEach(assemblyGroup.AssemblyPathSelector,
            assemblyPathSelector => EvaluateAssemblyPathSelector(root, assemblyPathSelector));

        Parallel.ForEach(assemblyGroup.AssemblyPath,
            assemblyPath => LoadAssemblies($@"{root.Path}{assemblyPath.Path}", assemblyPath));
    }

    private void EvaluateAssemblyPathSelector(AssemblyRoot root, AssemblyPathSelector assemblyPathSelector)
    {
        var directories = Directory.GetDirectories(root.Path)
            .Where(directory => Regex.IsMatch(directory, assemblyPathSelector.Selector))
            .Select(directory => $@"{directory}\{assemblyPathSelector.Path}");

        Parallel.ForEach(directories, directory => LoadAssemblies(directory, assemblyPathSelector));
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

    private void LoadAssemblies(string absolutPath, IAssemblyHolder assemblyHolder)
    {
        if (Directory.Exists(absolutPath))
        {
            var assemblyDirectory = new AssemblyDirectory(assemblyHolder.Path, absolutPath, Directory.GetFiles(absolutPath.Trim(), "*.dll"));

            var filteredFiles = assemblyDirectory.Files
                .Where(file => _includeFileRegexPatterns
                    .Any(pattern => pattern.IsMatch(file)))
                .ToList();

            Log.Debug("Loading {0} assemblies", filteredFiles.Count);

            Parallel.ForEach(filteredFiles, file =>
            {
                try
                {
                    var assembly = Assembly.LoadFrom(file);
                    assemblyDirectory.Assemblies.Add(assembly);
                }
                catch (Exception)
                {
                    // _logger.Warn($"{Path.GetFileName(file)}: {e.Message}");
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