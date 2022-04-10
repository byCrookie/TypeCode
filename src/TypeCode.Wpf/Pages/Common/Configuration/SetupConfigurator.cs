using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using TypeCode.Business.Configuration;
using TypeCode.Wpf.Helper.Navigation.Service;
using TypeCode.Wpf.Helper.Navigation.Wizard.WizardSimple;
using TypeCode.Wpf.Pages.Common.Configuration.AssemblyGroupWizard;
using TypeCode.Wpf.Pages.Common.Configuration.AssemblyPathSelectorWizard;
using TypeCode.Wpf.Pages.Common.Configuration.AssemblyPathWizard;
using TypeCode.Wpf.Pages.Common.Configuration.AssemblyRootWizard;
using TypeCode.Wpf.Pages.Common.Configuration.IncludeAssemblyPatternWizard;

namespace TypeCode.Wpf.Pages.Common.Configuration;

internal class SetupConfigurator : ISetupConfigurator
{
    private readonly IGenericXmlSerializer _genericXmlSerializer;
    private readonly IConfigurationMapper _configurationMapper;
    private readonly IWizardNavigationService _wizardNavigationService;
    private TypeCodeConfiguration _configuration;

    private readonly IDictionary<TreeViewItem, AssemblyRoot> _setupRootMappings = new Dictionary<TreeViewItem, AssemblyRoot>();
    private readonly IDictionary<TreeViewItem, AssemblyGroup> _setupGroupMappings = new Dictionary<TreeViewItem, AssemblyGroup>();

    public SetupConfigurator(
        IGenericXmlSerializer genericXmlSerializer,
        IConfigurationMapper configurationMapper,
        IWizardNavigationService wizardNavigationService
    )
    {
        _genericXmlSerializer = genericXmlSerializer;
        _configurationMapper = configurationMapper;
        _wizardNavigationService = wizardNavigationService;

        _configuration = new TypeCodeConfiguration();
    }

    public async Task<TreeViewItem> InitializeAsync()
    {
        var xmlConfiguration = await ReadXmlConfigurationAsync().ConfigureAwait(true);
        _configuration = _configurationMapper.MapToConfiguration(xmlConfiguration);
        return await BuildTreeViewAsync(_configuration).ConfigureAwait(true);
    }

    public async Task AddRootAsync(TreeViewItem parentItem)
    {
        var result = await _wizardNavigationService.OpenWizardAsync(new WizardParameter<AssemblyRootWizardViewModel>
        {
            FinishButtonText = "Add"
        }, new NavigationContext()).ConfigureAwait(true);

        var newItem = new TreeViewItem
        {
            Header = $"AssemblyRoot Path={result.Path} Priority={result.Priority}"
        };

        parentItem.Items.Add(newItem);

        var newAssemblyRoot = new AssemblyRoot
        {
            Path = result.Path ?? throw new Exception(),
            Priority = result.Priority ?? throw new Exception()
        };

        _configuration.AssemblyRoot.Add(newAssemblyRoot);

        _setupRootMappings.Add(newItem, newAssemblyRoot);
    }

    public bool CanAddRoot(TreeViewItem? parentItem)
    {
        return parentItem is not null && $"{parentItem.Header}".StartsWith("TypeCodeConfiguration");
    }

    public async Task AddGroupAsync(TreeViewItem parentItem)
    {
        var parentRoot = _setupRootMappings[parentItem];
        
        var result = await _wizardNavigationService.OpenWizardAsync(new WizardParameter<AssemblyGroupWizardViewModel>
        {
            FinishButtonText = "Add"
        }, new NavigationContext()).ConfigureAwait(true);

        var newItem = new TreeViewItem
        {
            Header = $"AssemblyGroup Name={result.Name} Priority={result.Priority}",
            IsExpanded = true
        };

        parentItem.Items.Add(newItem);

        var newAssemblyGroup = new AssemblyGroup
        {
            Name = result.Name ?? throw new Exception(),
            Priority = result.Priority ?? throw new Exception()
        };

        parentRoot.AssemblyGroup.Add(newAssemblyGroup);

        _setupGroupMappings.Add(newItem, newAssemblyGroup);
    }

    public bool CanAddGroup(TreeViewItem? parentItem)
    {
        return parentItem is not null && $"{parentItem.Header}".StartsWith("AssemblyRoot");
    }

    public async Task AddPathAsync(TreeViewItem parentItem)
    {
        var parentGroup = _setupGroupMappings[parentItem];
        
        var result = await _wizardNavigationService.OpenWizardAsync(new WizardParameter<AssemblyPathWizardViewModel>
        {
            FinishButtonText = "Add"
        }, new NavigationContext()).ConfigureAwait(true);

        var newItem = new TreeViewItem
        {
            Header = $"AssemblyPath Priority={result.Priority} Value={result.Path}",
            IsExpanded = true
        };

        parentItem.Items.Add(newItem);

        var newAssemblyPath = new AssemblyPath
        {
            Path = result.Path ?? throw new Exception(),
            Priority = result.Priority ?? throw new Exception()
        };

        parentGroup.AssemblyPath.Add(newAssemblyPath);
    }

    public bool CanAddPath(TreeViewItem? parentItem)
    {
        return parentItem is not null && $"{parentItem.Header}".StartsWith("AssemblyGroup");
    }

    public async Task AddIncludePatternAsync(TreeViewItem parentItem)
    {
        var parentRoot = _setupRootMappings[parentItem];
        
        var result = await _wizardNavigationService.OpenWizardAsync(new WizardParameter<IncludeAssemblyPatternWizardViewModel>
        {
            FinishButtonText = "Add"
        }, new NavigationContext()).ConfigureAwait(true);

        var newItem = new TreeViewItem
        {
            Header = $"IncludeAssemblyPattern Value={result.Pattern}",
            IsExpanded = true
        };

        parentItem.Items.Add(newItem);

        parentRoot.IncludeAssemblyPattern.Add(new Regex(result.Pattern ?? string.Empty, RegexOptions.Compiled));
    }

    public bool CanAddIncludePattern(TreeViewItem? parentItem)
    {
        return parentItem is not null && $"{parentItem.Header}".StartsWith("AssemblyRoot");
    }

    public async Task AddSelectorAsync(TreeViewItem parentItem)
    {
        var parentGroup = _setupGroupMappings[parentItem];
        
        var result = await _wizardNavigationService.OpenWizardAsync(new WizardParameter<AssemblyPathSelectorWizardViewModel>
        {
            FinishButtonText = "Add"
        }, new NavigationContext()).ConfigureAwait(true);

        var newItem = new TreeViewItem
        {
            Header = $"AssemblyPathSelector Priority={result.Priority} Value={result.Selector}",
            IsExpanded = true
        };

        parentItem.Items.Add(newItem);

        var newAssemblyPathSelector = new AssemblyPathSelector
        {
            Selector = result.Selector ?? throw new Exception(),
            Priority = result.Priority ?? throw new Exception()
        };

        parentGroup.AssemblyPathSelector.Add(newAssemblyPathSelector);
    }

    public bool CanAddSelector(TreeViewItem? parentItem)
    {
        return parentItem is not null && $"{parentItem.Header}".StartsWith("AssemblyGroup");
    }

    public Task ExportAsync()
    {
        var xmlConfiguration = _configurationMapper.MapToXml(_configuration);
        return WriteXmlConfigurationAsync(xmlConfiguration);
    }

    private Task<TreeViewItem> BuildTreeViewAsync(TypeCodeConfiguration configuration)
    {
        return BuildTypeCodeConfigurationAsync(configuration);
    }

    private Task<TreeViewItem> BuildTypeCodeConfigurationAsync(TypeCodeConfiguration configuration)
    {
        var typeCodeConfigurationItem = new TreeViewItem
        {
            Header = $"TypeCodeConfiguration CloseCmd={configuration.CloseCmd}",
            IsExpanded = true
        };

        BuildAssemblyRoots(configuration, typeCodeConfigurationItem);

        return Task.FromResult(typeCodeConfigurationItem);
    }

    private void BuildAssemblyRoots(TypeCodeConfiguration typeCodeConfiguration, TreeViewItem typeCodeConfigurationItem)
    {
        foreach (var assemblyRoot in typeCodeConfiguration.AssemblyRoot)
        {
            var assemblyRootItem = new TreeViewItem
            {
                Header = $"AssemblyRoot Path={assemblyRoot.Path} Priority={assemblyRoot.Priority}",
                IsExpanded = true
            };

            BuildIncludeAssemblyPatterns(assemblyRoot, assemblyRootItem);

            BuildAssemblyGroups(assemblyRoot, assemblyRootItem);

            typeCodeConfigurationItem.Items.Add(assemblyRootItem);
            
            _setupRootMappings.Add(assemblyRootItem, assemblyRoot);
        }
    }

    private void BuildAssemblyGroups(AssemblyRoot assemblyRoot, TreeViewItem assemblyRootItem)
    {
        foreach (var assemblyGroup in assemblyRoot.AssemblyGroup)
        {
            var assemblyGroupItem = new TreeViewItem
            {
                Header = $"AssemblyGroup Name={assemblyGroup.Name} Priority={assemblyGroup.Priority}",
                IsExpanded = true
            };

            BuildAssemblyPaths(assemblyGroup, assemblyGroupItem);
            
            BuildAssemblyPathSelectors(assemblyGroup, assemblyGroupItem);

            assemblyRootItem.Items.Add(assemblyGroupItem);
            
            _setupGroupMappings.Add(assemblyGroupItem, assemblyGroup);
        }
    }

    private static void BuildIncludeAssemblyPatterns(AssemblyRoot assemblyRoot, TreeViewItem assemblyRootItem)
    {
        foreach (var includeAssemblyPattern in assemblyRoot.IncludeAssemblyPattern)
        {
            var includeAssemblyPatternItem = new TreeViewItem
            {
                Header = $"IncludeAssemblyPattern Value={includeAssemblyPattern}",
                IsExpanded = true
            };

            assemblyRootItem.Items.Add(includeAssemblyPatternItem);
        }
    }

    private static void BuildAssemblyPaths(AssemblyGroup assemblyGroup, TreeViewItem assemblyGroupItem)
    {
        foreach (var assemblyPath in assemblyGroup.AssemblyPath)
        {
            var assemblyPathItem = new TreeViewItem
            {
                Header = $"AssemblyPath Priority={assemblyPath.Priority} Value={assemblyPath.Path}",
                IsExpanded = true
            };

            assemblyGroupItem.Items.Add(assemblyPathItem);
        }
    }
    
    private static void BuildAssemblyPathSelectors(AssemblyGroup assemblyGroup, TreeViewItem assemblyGroupItem)
    {
        foreach (var assemblyPathSelector in assemblyGroup.AssemblyPathSelector)
        {
            var assemblyPathSelectorItem = new TreeViewItem
            {
                Header = $"AssemblyPathSelector Priority={assemblyPathSelector.Priority} Value={assemblyPathSelector.Selector}",
                IsExpanded = true
            };

            assemblyGroupItem.Items.Add(assemblyPathSelectorItem);
        }
    }

    private async Task<XmlTypeCodeConfiguration> ReadXmlConfigurationAsync()
    {
        var cfg = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\Configuration.cfg.xml";
        var xml = await File.ReadAllTextAsync(cfg).ConfigureAwait(true);
        return _genericXmlSerializer.Deserialize<XmlTypeCodeConfiguration>(xml) ?? throw new Exception($"{cfg} can not be parsed");
    }

    private Task WriteXmlConfigurationAsync(XmlTypeCodeConfiguration xmlConfiguration)
    {
        var cfg = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\Configuration.cfg.xml";
        var serialized = _genericXmlSerializer.Serialize(xmlConfiguration);
        return File.WriteAllTextAsync(cfg, serialized);
    }
}