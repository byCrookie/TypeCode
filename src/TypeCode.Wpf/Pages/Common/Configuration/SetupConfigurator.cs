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
        var result = await OpenWizardAsync<AssemblyRootWizardViewModel>().ConfigureAwait(false);
        var newItem = CreateAssemblyRootItem(result.Path ?? string.Empty, result.Priority.GetValueOrDefault());
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
        return parentItem is not null && $"{parentItem.Header}".StartsWith(TypeCodeConfigurationName);
    }

    public async Task AddGroupAsync(TreeViewItem parentItem)
    {
        var parentRoot = _setupRootMappings[parentItem];
        var result = await OpenWizardAsync<AssemblyGroupWizardViewModel>().ConfigureAwait(false);
        var newItem = CreateAssemblyGroupItem(result.Name ?? string.Empty, result.Priority.GetValueOrDefault());
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
        return parentItem is not null && $"{parentItem.Header}".StartsWith(AssemblyRootName);
    }

    public async Task AddPathAsync(TreeViewItem parentItem)
    {
        var parentGroup = _setupGroupMappings[parentItem];
        var result = await OpenWizardAsync<AssemblyPathWizardViewModel>().ConfigureAwait(false);
        var newItem = CreateAssemblyPathItem(result.Priority.GetValueOrDefault(), result.Path ?? string.Empty);
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
        return parentItem is not null && $"{parentItem.Header}".StartsWith(AssemblyGroupName);
    }

    public async Task AddIncludePatternAsync(TreeViewItem parentItem)
    {
        var parentRoot = _setupRootMappings[parentItem];
        var result = await OpenWizardAsync<IncludeAssemblyPatternWizardViewModel>().ConfigureAwait(false);
        var newItem = CreateIncludeAssemblyPatternItem(result.Pattern ?? string.Empty);
        parentItem.Items.Add(newItem);
        parentRoot.IncludeAssemblyPattern.Add(new Regex(result.Pattern ?? string.Empty, RegexOptions.Compiled));
    }

    public bool CanAddIncludePattern(TreeViewItem? parentItem)
    {
        return parentItem is not null && $"{parentItem.Header}".StartsWith(AssemblyRootName);
    }

    public async Task AddSelectorAsync(TreeViewItem parentItem)
    {
        var parentGroup = _setupGroupMappings[parentItem];
        var result = await OpenWizardAsync<AssemblyPathSelectorWizardViewModel>().ConfigureAwait(false);
        var newItem = CreateAssemblyPathSelectorItem(result.Priority.GetValueOrDefault(), result.Selector ?? string.Empty);
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
        return parentItem is not null && $"{parentItem.Header}".StartsWith(AssemblyGroupName);
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
        var typeCodeConfigurationItem = CreateTypeCodeConfigurationItem(configuration.CloseCmd);
        BuildAssemblyRoots(configuration, typeCodeConfigurationItem);
        return Task.FromResult(typeCodeConfigurationItem);
    }

    private void BuildAssemblyRoots(TypeCodeConfiguration typeCodeConfiguration, ItemsControl typeCodeConfigurationItem)
    {
        foreach (var assemblyRoot in typeCodeConfiguration.AssemblyRoot)
        {
            var assemblyRootItem = CreateAssemblyRootItem(assemblyRoot.Path, assemblyRoot.Priority);
            BuildIncludeAssemblyPatterns(assemblyRoot, assemblyRootItem);
            BuildAssemblyGroups(assemblyRoot, assemblyRootItem);
            typeCodeConfigurationItem.Items.Add(assemblyRootItem);
            _setupRootMappings.Add(assemblyRootItem, assemblyRoot);
        }
    }

    private void BuildAssemblyGroups(AssemblyRoot assemblyRoot, ItemsControl assemblyRootItem)
    {
        foreach (var assemblyGroup in assemblyRoot.AssemblyGroup)
        {
            var assemblyGroupItem = CreateAssemblyGroupItem(assemblyGroup.Name, assemblyGroup.Priority);
            BuildAssemblyPaths(assemblyGroup, assemblyGroupItem);
            BuildAssemblyPathSelectors(assemblyGroup, assemblyGroupItem);
            assemblyRootItem.Items.Add(assemblyGroupItem);
            _setupGroupMappings.Add(assemblyGroupItem, assemblyGroup);
        }
    }

    private static void BuildIncludeAssemblyPatterns(AssemblyRoot assemblyRoot, ItemsControl assemblyRootItem)
    {
        foreach (var includeAssemblyPatternItem in assemblyRoot.IncludeAssemblyPattern
                     .Select(pattern => CreateIncludeAssemblyPatternItem($"{pattern}")))
        {
            assemblyRootItem.Items.Add(includeAssemblyPatternItem);
        }
    }

    private static void BuildAssemblyPaths(AssemblyGroup assemblyGroup, ItemsControl assemblyGroupItem)
    {
        foreach (var assemblyPathItem in assemblyGroup.AssemblyPath
                     .Select(assemblyPath => CreateAssemblyPathItem(assemblyPath.Priority, assemblyPath.Path)))
        {
            assemblyGroupItem.Items.Add(assemblyPathItem);
        }
    }

    private static void BuildAssemblyPathSelectors(AssemblyGroup assemblyGroup, ItemsControl assemblyGroupItem)
    {
        foreach (var assemblyPathSelectorItem in assemblyGroup.AssemblyPathSelector
                     .Select(assemblyPathSelector => CreateAssemblyPathSelectorItem(assemblyPathSelector.Priority, assemblyPathSelector.Selector)))
        {
            assemblyGroupItem.Items.Add(assemblyPathSelectorItem);
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

    private Task<T> OpenWizardAsync<T>() where T : notnull
    {
        return _wizardNavigationService.OpenWizardAsync(new WizardParameter<T>
        {
            FinishButtonText = "Add"
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