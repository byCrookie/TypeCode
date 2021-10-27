using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Framework.Boot;
using Framework.Extensions.List;
using Framework.Workflow;
using TypeCode.Business.Configuration;
using TypeCode.Business.Format;

namespace TypeCode.Business.Bootstrapping
{
    internal class AssemblyLoadBootStep<TContext> : IAssemblyLoadBootStep<TContext>
        where TContext : WorkflowBaseContext, IBootContext
    {
        private IEnumerable<Regex> _includeFileRegexPatterns;
        // private readonly ILog _logger = LogManager.GetLogger(typeof(AssemblyLoadBootStep<TContext>));

        public Task ExecuteAsync(TContext context)
        {
            // _logger.Info("Evaluating .dll files");

            var xmlConfiguration = ReadXmlConfiguration();
            var configuration = MapToConfiguration(xmlConfiguration);

            Parallel.ForEach(configuration.AssemblyRoot, EvaluateAssemblyRoot);

            Console.WriteLine($@"{Cuts.Short()} Assembly Priority");

            foreach (var root in configuration.AssemblyRoot.OrderBy(r => r.Priority))
            {
                foreach (var group in root.AssemblyGroup)
                {
                    var messages = new List<PriorityString>();

                    group.AssemblyPathSelector.ForEach(selector =>
                    {
                        selector.AssemblyDirectories
                            .ForEach(directory => messages
                                .Add(new PriorityString(selector.Priority,
                                    $@"{Cuts.Point()} {directory.AbsolutPath}")));
                    });

                    group.AssemblyPath.ForEach(path =>
                    {
                        path.AssemblyDirectories
                            .ForEach(directory => messages
                                .Add(new PriorityString(path.Priority, $@"{Cuts.Point()} {directory.AbsolutPath}")));
                    });

                    foreach (var message in messages.OrderBy(message => message.Priority).ToList())
                    {
                        Console.WriteLine(message.Message);
                    }
                }
            }

            // _logger.Info($"Total of {CountAssemblies(configuration)} assemblies have been loaded");
            var assemblyProvider = new ConfigurationProvider();
            assemblyProvider.SetConfiguration(configuration);
            AssemblyLoadProvider.SetAssemblyProvider(assemblyProvider);
            return Task.CompletedTask;
        }

        public Task<bool> ShouldExecuteAsync(TContext context)
        {
            return Task.FromResult(true);
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

        // private static int CountAssemblies(TypeCodeConfiguration configuration)
        // {
        //     return configuration.AssemblyRoot
        //                .Sum(root => root.AssemblyGroup
        //                    .Sum(group => group.AssemblyPath
        //                        .Sum(path => path.AssemblyDirectories
        //                            .Sum(directory => directory.Assemblies.Count))))
        //            +
        //            configuration.AssemblyRoot
        //                .Sum(root => root.AssemblyGroup
        //                    .Sum(group => group.AssemblyPathSelector
        //                        .Sum(path => path.AssemblyDirectories
        //                            .Sum(directory => directory.Assemblies.Count))));
        // }

        private void LoadAssemblies(string absolutPath, IAssemblyHolder assemblyHolder)
        {
            if (Directory.Exists(absolutPath))
            {
                var assemblyDirectory = new AssemblyDirectory
                {
                    RelativePath = assemblyHolder.Path,
                    AbsolutPath = absolutPath,
                    Files = Directory.GetFiles(absolutPath.Trim(), "*.dll")
                };


                var filteredFiles = assemblyDirectory.Files
                    .Where(file => _includeFileRegexPatterns
                        .Any(pattern => pattern.IsMatch(file)))
                    .ToList();

                // _logger.Info($"Loading {filteredFiles.Count} assemblies");

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

                // _logger.Info($"Loaded {assemblyDirectory.Assemblies.Count} assemblies");

                assemblyHolder.AssemblyDirectories.Add(assemblyDirectory);
            }
        }

        private static TypeCodeConfiguration MapToConfiguration(XmlTypeCodeConfiguration xmlConfiguration)
        {
            return new()
            {
                AssemblyRoot = MapToConfiguration(xmlConfiguration.AssemblyRoot).ToList(),
                VersionPageName = xmlConfiguration.VersionPageName,
                SpaceKey = xmlConfiguration.SpaceKey,
                CloseCmd = xmlConfiguration.CloseCmd,
                BaseUrl = xmlConfiguration.BaseUrl,
                Username = xmlConfiguration.Username,
                Password = xmlConfiguration.Password
            };
        }

        private static IEnumerable<AssemblyRoot> MapToConfiguration(IEnumerable<XmlAssemblyRoot> xmlAssemblyRoots)
        {
            return xmlAssemblyRoots.Select(root => new AssemblyRoot
            {
                Priority = root.Priority,
                Path = root.Path,
                Text = root.Text,
                AssemblyGroup = MapToConfiguration(root.AssemblyGroup).ToList(),
                IncludeAssemblyPattern = root.IncludeAssemblyPattern
            });
        }

        private static IEnumerable<AssemblyGroup> MapToConfiguration(
            IEnumerable<XmlAssemblyGroup> xmlConfigurationAssemblyGroups)
        {
            return xmlConfigurationAssemblyGroups.Select(xmlConfigurationAssemblyGroup => new AssemblyGroup
            {
                Name = xmlConfigurationAssemblyGroup.Name,
                Priority = xmlConfigurationAssemblyGroup.Priority,
                AssemblyPath = MapToConfiguration(xmlConfigurationAssemblyGroup.AssemblyPath).ToList(),
                AssemblyPathSelector = MapToConfiguration(xmlConfigurationAssemblyGroup.AssemblyPathSelector).ToList()
            });
        }

        private static IEnumerable<AssemblyPathSelector> MapToConfiguration(
            IEnumerable<XmlAssemblyPathSelector> xmlAssemblyPathSelectors)
        {
            return xmlAssemblyPathSelectors.Select(xmlAssemblyPathSelector => new AssemblyPathSelector
            {
                Path = xmlAssemblyPathSelector.Text,
                Priority = xmlAssemblyPathSelector.Priority,
                Selector = xmlAssemblyPathSelector.Selector
            });
        }

        private static IEnumerable<AssemblyPath> MapToConfiguration(IEnumerable<XmlAssemblyPath> xmlAssemblyPaths)
        {
            return xmlAssemblyPaths.Select(xmlAssemblyPath => new AssemblyPath
            {
                Path = xmlAssemblyPath.Text,
                Priority = xmlAssemblyPath.Priority
            });
        }

        private static XmlTypeCodeConfiguration ReadXmlConfiguration()
        {
            var cfg = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\Configuration.cfg.xml";
            var xml = File.ReadAllText(cfg);
            return GenericXmlSerializer.Deserialize<XmlTypeCodeConfiguration>(xml);
        }
    }
}