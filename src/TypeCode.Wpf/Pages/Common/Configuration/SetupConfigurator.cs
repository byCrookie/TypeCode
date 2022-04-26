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

    private readonly IDictionary<TreeViewItem, SetupTreeViewItemMapping<AssemblyRoot>> _setupRootMappings
        = new Dictionary<TreeViewItem, SetupTreeViewItemMapping<AssemblyRoot>>();

    private readonly IDictionary<TreeViewItem, SetupTreeViewItemMapping<AssemblyGroup>> _setupGroupMappings
        = new Dictionary<TreeViewItem, SetupTreeViewItemMapping<AssemblyGroup>>();

    private readonly IDictionary<TreeViewItem, SetupTreeViewItemMapping<AssemblyPath>> _setupPathMappings
        = new Dictionary<TreeViewItem, SetupTreeViewItemMapping<AssemblyPath>>();

    private readonly IDictionary<TreeViewItem, SetupTreeViewItemMapping<AssemblyPathSelector>> _setupPathSelectorMappings
        = new Dictionary<TreeViewItem, SetupTreeViewItemMapping<AssemblyPathSelector>>();

    private readonly IDictionary<TreeViewItem, SetupTreeViewItemMapping<Regex>> _setupIncludePatternMappings
        = new Dictionary<TreeViewItem, SetupTreeViewItemMapping<Regex>>();

    private const string TypeCodeConfigurationName = "TypeCodeConfiguration";
    private const string AssemblyRootName = "AssemblyRoot";
    private const string IncludeAssemblyPatternName = "IncludeAssemblyPattern";
    private const string AssemblyGroupName = "AssemblyGroup";
    private const string AssemblyPathName = "AssemblyPath";
    private const string AssemblyPathSelectorName = "AssemblyPathSelector";

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
        var result = await OpenWizardAsync<AssemblyRootWizardViewModel>().ConfigureAwait(true);

        if (string.IsNullOrEmpty(result.Path) || result.Priority is null)
        {
            return;
        }
        
        var newItem = CreateAssemblyRootItem(result.Path, result.Priority.Value);
        parentItem.Items.Add(newItem);
        var newAssemblyRoot = new AssemblyRoot
        {
            Path = result.Path ?? throw new Exception(),
            Priority = result.Priority ?? throw new Exception()
        };
        _configuration.AssemblyRoot.Add(newAssemblyRoot);
        _setupRootMappings.Add(newItem, new SetupTreeViewItemMapping<AssemblyRoot>(newItem, parentItem, newAssemblyRoot));
    }

    public bool CanAddRoot(TreeViewItem? parentItem)
    {
        return parentItem is not null && $"{parentItem.Header}".StartsWith(TypeCodeConfigurationName);
    }

    public async Task AddGroupAsync(TreeViewItem parentItem)
    {
        var parentRoot = _setupRootMappings[parentItem];
        var result = await OpenWizardAsync<AssemblyGroupWizardViewModel>().ConfigureAwait(true);
        
        if (string.IsNullOrEmpty(result.Name) || result.Priority is null)
        {
            return;
        }
        
        var newItem = CreateAssemblyGroupItem(result.Name, result.Priority.Value);
        parentItem.Items.Add(newItem);
        var newAssemblyGroup = new AssemblyGroup
        {
            Name = result.Name ?? throw new Exception(),
            Priority = result.Priority ?? throw new Exception()
        };
        parentRoot.Type.AssemblyGroup.Add(newAssemblyGroup);
        _setupGroupMappings.Add(newItem, new SetupTreeViewItemMapping<AssemblyGroup>(newItem, parentItem, newAssemblyGroup));
    }

    public bool CanAddGroup(TreeViewItem? parentItem)
    {
        return parentItem is not null && $"{parentItem.Header}".StartsWith(AssemblyRootName);
    }

    public async Task AddPathAsync(TreeViewItem parentItem)
    {
        var parentGroup = _setupGroupMappings[parentItem];
        var result = await OpenWizardAsync<AssemblyPathWizardViewModel>().ConfigureAwait(true);
        
        if (string.IsNullOrEmpty(result.Path) || result.Priority is null)
        {
            return;
        }
        
        var newItem = CreateAssemblyPathItem(result.Priority.Value, result.Path);
        parentItem.Items.Add(newItem);
        var newAssemblyPath = new AssemblyPath
        {
            Path = result.Path ?? throw new Exception(),
            Priority = result.Priority ?? throw new Exception()
        };
        parentGroup.Type.AssemblyPath.Add(newAssemblyPath);
        _setupPathMappings.Add(newItem, new SetupTreeViewItemMapping<AssemblyPath>(newItem, parentItem, newAssemblyPath));
    }

    public bool CanAddPath(TreeViewItem? parentItem)
    {
        return parentItem is not null && $"{parentItem.Header}".StartsWith(AssemblyGroupName);
    }

    public async Task AddIncludePatternAsync(TreeViewItem parentItem)
    {
        var parentRoot = _setupRootMappings[parentItem];
        var result = await OpenWizardAsync<IncludeAssemblyPatternWizardViewModel>().ConfigureAwait(true);
        
        if (string.IsNullOrEmpty(result.Pattern))
        {
            return;
        }
        
        var newItem = CreateIncludeAssemblyPatternItem(result.Pattern);
        parentItem.Items.Add(newItem);
        var pattern = new Regex(result.Pattern ?? string.Empty, RegexOptions.Compiled);
        parentRoot.Type.IncludeAssemblyPattern.Add(pattern);
        _setupIncludePatternMappings.Add(newItem, new SetupTreeViewItemMapping<Regex>(newItem, parentItem, pattern));
    }

    public bool CanAddIncludePattern(TreeViewItem? parentItem)
    {
        return parentItem is not null && $"{parentItem.Header}".StartsWith(AssemblyRootName);
    }

    public async Task AddSelectorAsync(TreeViewItem parentItem)
    {
        var parentGroup = _setupGroupMappings[parentItem];
        var result = await OpenWizardAsync<AssemblyPathSelectorWizardViewModel>().ConfigureAwait(true);
        
        if (string.IsNullOrEmpty(result.Selector) || result.Priority is null)
        {
            return;
        }
        
        var newItem = CreateAssemblyPathSelectorItem(result.Priority.Value, result.Selector);
        parentItem.Items.Add(newItem);
        var newAssemblyPathSelector = new AssemblyPathSelector
        {
            Selector = result.Selector ?? throw new Exception(),
            Priority = result.Priority ?? throw new Exception()
        };
        parentGroup.Type.AssemblyPathSelector.Add(newAssemblyPathSelector);
        _setupPathSelectorMappings.Add(newItem, new SetupTreeViewItemMapping<AssemblyPathSelector>(newItem, parentItem, newAssemblyPathSelector));
    }

    public bool CanAddSelector(TreeViewItem? parentItem)
    {
        return parentItem is not null && $"{parentItem.Header}".StartsWith(AssemblyGroupName);
    }

    public Task ExportAsync()
    {
        var xmlConfiguration = _configurationMapper.MapToXml(_configuration);
        return WriteXmlConfigurationAsync(xmlConfiguration);
    }

    public async Task UpdateAsync(TreeViewItem selectedItem)
    {
        // if (_setupRootMappings.ContainsKey(selectedItem))
        // {
        //     var mapping = _setupRootMappings[selectedItem];
        //     DeleteRoot(selectedItem);
        //     await AddRootAsync(mapping.ParentItem, model =>
        //     {
        //         model.Path = mapping.Type.Path;
        //         model.Priority = mapping.Type.Priority;
        //         return Task.CompletedTask;
        //     }).ConfigureAwait(true);
        // }
    }

    public bool CanUpdate(TreeViewItem? selectedItem)
    {
        return selectedItem is not null && !$"{selectedItem.Header}".StartsWith(TypeCodeConfigurationName);
    }

    public Task DeleteAsync(TreeViewItem selectedItem)
    {
        DeleteRoot(selectedItem);
        DeleteGroup(selectedItem);
        DeletePath(selectedItem);
        DeletePathSelector(selectedItem);
        DeleteIncludePattern(selectedItem);
        return Task.CompletedTask;
    }

    private void DeleteIncludePattern(TreeViewItem selectedItem)
    {
        if (_setupIncludePatternMappings.ContainsKey(selectedItem))
        {
            var mapping = _setupIncludePatternMappings[selectedItem];
            var parentMapping = _setupRootMappings[mapping.ParentItem];
            parentMapping.Type.IncludeAssemblyPattern.Remove(mapping.Type);
            mapping.ParentItem.Items.Remove(selectedItem);
            _setupIncludePatternMappings.Remove(selectedItem);
        }
    }

    private void DeletePathSelector(TreeViewItem selectedItem)
    {
        if (_setupPathSelectorMappings.ContainsKey(selectedItem))
        {
            var mapping = _setupPathSelectorMappings[selectedItem];
            var parentMapping = _setupGroupMappings[mapping.ParentItem];
            parentMapping.Type.AssemblyPathSelector.Remove(mapping.Type);
            mapping.ParentItem.Items.Remove(selectedItem);
            _setupPathSelectorMappings.Remove(selectedItem);
        }
    }

    private void DeletePath(TreeViewItem selectedItem)
    {
        if (_setupPathMappings.ContainsKey(selectedItem))
        {
            var mapping = _setupPathMappings[selectedItem];
            var parentMapping = _setupGroupMappings[mapping.ParentItem];
            parentMapping.Type.AssemblyPath.Remove(mapping.Type);
            mapping.ParentItem.Items.Remove(selectedItem);
            _setupPathMappings.Remove(selectedItem);
        }
    }

    private void DeleteGroup(TreeViewItem selectedItem)
    {
        if (_setupGroupMappings.ContainsKey(selectedItem))
        {
            var mapping = _setupGroupMappings[selectedItem];
            var parentMapping = _setupRootMappings[mapping.ParentItem];
            parentMapping.Type.AssemblyGroup.Remove(mapping.Type);
            mapping.ParentItem.Items.Remove(selectedItem);
            _setupGroupMappings.Remove(selectedItem);
        }
    }

    private void DeleteRoot(TreeViewItem selectedItem)
    {
        if (_setupRootMappings.ContainsKey(selectedItem))
        {
            var mapping = _setupRootMappings[selectedItem];
            _configuration.AssemblyRoot.Remove(mapping.Type);
            mapping.ParentItem.Items.Remove(selectedItem);
            _setupRootMappings.Remove(selectedItem);
        }
    }

    public bool CanDelete(TreeViewItem? selectedItem)
    {
        return selectedItem is not null && !$"{selectedItem.Header}".StartsWith(TypeCodeConfigurationName);
    }

    private Task<TreeViewItem> BuildTreeViewAsync(TypeCodeConfiguration configuration)
    {
        return BuildTypeCodeConfigurationAsync(configuration);
    }

    private Task<TreeViewItem> BuildTypeCodeConfigurationAsync(TypeCodeConfiguration configuration)
    {
        var typeCodeConfigurationItem = CreateTypeCodeConfigurationItem(configuration.CloseCmd);
        BuildAssemblyRoots(configuration, typeCodeConfigurationItem);
        return Task.FromResult(typeCodeConfigurationItem);
    }

    private void BuildAssemblyRoots(TypeCodeConfiguration typeCodeConfiguration, TreeViewItem typeCodeConfigurationItem)
    {
        foreach (var assemblyRoot in typeCodeConfiguration.AssemblyRoot)
        {
            var assemblyRootItem = CreateAssemblyRootItem(assemblyRoot.Path, assemblyRoot.Priority);
            BuildIncludeAssemblyPatterns(assemblyRoot, assemblyRootItem);
            BuildAssemblyGroups(assemblyRoot, assemblyRootItem);
            typeCodeConfigurationItem.Items.Add(assemblyRootItem);
            _setupRootMappings.Add(assemblyRootItem, new SetupTreeViewItemMapping<AssemblyRoot>(assemblyRootItem, typeCodeConfigurationItem, assemblyRoot));
        }
    }

    private void BuildAssemblyGroups(AssemblyRoot assemblyRoot, TreeViewItem assemblyRootItem)
    {
        foreach (var assemblyGroup in assemblyRoot.AssemblyGroup)
        {
            var assemblyGroupItem = CreateAssemblyGroupItem(assemblyGroup.Name, assemblyGroup.Priority);
            BuildAssemblyPaths(assemblyGroup, assemblyGroupItem);
            BuildAssemblyPathSelectors(assemblyGroup, assemblyGroupItem);
            assemblyRootItem.Items.Add(assemblyGroupItem);
            _setupGroupMappings.Add(assemblyGroupItem, new SetupTreeViewItemMapping<AssemblyGroup>(assemblyGroupItem, assemblyRootItem, assemblyGroup));
        }
    }

    private void BuildIncludeAssemblyPatterns(AssemblyRoot assemblyRoot, TreeViewItem assemblyRootItem)
    {
        foreach (var includeAssemblyPattern in assemblyRoot.IncludeAssemblyPattern)
        {
            var includeAssemblyPatternItem = CreateIncludeAssemblyPatternItem($"{includeAssemblyPattern}");
            assemblyRootItem.Items.Add(includeAssemblyPatternItem);
            _setupIncludePatternMappings.Add(includeAssemblyPatternItem, new SetupTreeViewItemMapping<Regex>(includeAssemblyPatternItem, assemblyRootItem, includeAssemblyPattern));
        }
    }

    private void BuildAssemblyPaths(AssemblyGroup assemblyGroup, TreeViewItem assemblyGroupItem)
    {
        foreach (var assemblyPath in assemblyGroup.AssemblyPath)
        {
            var assemblyPathItem = CreateAssemblyPathItem(assemblyPath.Priority, assemblyPath.Path);
            assemblyGroupItem.Items.Add(assemblyPathItem);
            _setupPathMappings.Add(assemblyPathItem, new SetupTreeViewItemMapping<AssemblyPath>(assemblyPathItem, assemblyGroupItem, assemblyPath));
        }
    }

    private void BuildAssemblyPathSelectors(AssemblyGroup assemblyGroup, TreeViewItem assemblyGroupItem)
    {
        foreach (var assemblyPathSelector in assemblyGroup.AssemblyPathSelector)
        {
            var assemblyPathSelectorItem = CreateAssemblyPathSelectorItem(assemblyPathSelector.Priority, assemblyPathSelector.Selector);
            assemblyGroupItem.Items.Add(assemblyPathSelectorItem);
            _setupPathSelectorMappings.Add(assemblyPathSelectorItem, new SetupTreeViewItemMapping<AssemblyPathSelector>(assemblyPathSelectorItem, assemblyGroupItem, assemblyPathSelector));
        }
    }

    private static TreeViewItem CreateAssemblyRootItem(string path, int priority)
    {
        return new TreeViewItem
        {
            Header = $"{AssemblyRootName} Path={path} Priority={priority}",
            IsExpanded = true
        };
    }

    private static TreeViewItem CreateTypeCodeConfigurationItem(bool closeCmd)
    {
        return new TreeViewItem
        {
            Header = $"{TypeCodeConfigurationName} CloseCmd={closeCmd}",
            IsExpanded = true
        };
    }

    private static TreeViewItem CreateAssemblyGroupItem(string name, int priority)
    {
        return new TreeViewItem
        {
            Header = $"{AssemblyGroupName} Name={name} Priority={priority}",
            IsExpanded = true
        };
    }

    private static TreeViewItem CreateIncludeAssemblyPatternItem(string includeAssemblyPattern)
    {
        return new TreeViewItem
        {
            Header = $"{IncludeAssemblyPatternName} Value={includeAssemblyPattern}",
            IsExpanded = true
        };
    }

    private static TreeViewItem CreateAssemblyPathItem(int priority, string path)
    {
        return new TreeViewItem
        {
            Header = $"{AssemblyPathName} Priority={priority} Value={path}",
            IsExpanded = true
        };
    }

    private static TreeViewItem CreateAssemblyPathSelectorItem(int priority, string selector)
    {
        return new TreeViewItem
        {
            Header = $"{AssemblyPathSelectorName} Priority={priority} Value={selector}",
            IsExpanded = true
        };
    }

    private Task<T> OpenWizardAsync<T>(Func<T, Task>? initialize = null) where T : notnull
    {
        return _wizardNavigationService.OpenWizardAsync(new WizardParameter<T>
        {
            FinishButtonText = "Add",
            InitializeAsync = initialize ?? new Func<T, Task>(_ => Task.CompletedTask)
        }, new NavigationContext());
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