﻿using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using DependencyInjection.Factory;
using TypeCode.Business.Bootstrapping.Data;
using TypeCode.Business.Configuration;
using TypeCode.Wpf.Helper.Navigation.Wizard;
using TypeCode.Wpf.Pages.Configuration.Simple.AssemblyGroupWizard;
using TypeCode.Wpf.Pages.Configuration.Simple.AssemblyPathSelectorWizard;
using TypeCode.Wpf.Pages.Configuration.Simple.AssemblyPathWizard;
using TypeCode.Wpf.Pages.Configuration.Simple.AssemblyRootWizard;
using TypeCode.Wpf.Pages.Configuration.Simple.IncludeAssemblyPatternWizard;

namespace TypeCode.Wpf.Pages.Configuration.Simple;

internal class SimpleConfigurator : ISimpleConfigurator
{
    private readonly IGenericXmlSerializer _genericXmlSerializer;
    private readonly IConfigurationMapper _configurationMapper;
    private readonly IFactory<IWizardBuilder> _wizardBuilderFactory;
    private readonly IWizardRunner _wizardRunner;
    private readonly IUserDataLocationProvider _userDataLocationProvider;
    private TypeCodeConfiguration _configuration;

    private readonly IDictionary<TreeViewItem, ConfigurationTreeViewItemMapping<AssemblyRoot>> _setupRootMappings
        = new Dictionary<TreeViewItem, ConfigurationTreeViewItemMapping<AssemblyRoot>>();

    private readonly IDictionary<TreeViewItem, ConfigurationTreeViewItemMapping<AssemblyGroup>> _setupGroupMappings
        = new Dictionary<TreeViewItem, ConfigurationTreeViewItemMapping<AssemblyGroup>>();

    private readonly IDictionary<TreeViewItem, ConfigurationTreeViewItemMapping<AssemblyPath>> _setupPathMappings
        = new Dictionary<TreeViewItem, ConfigurationTreeViewItemMapping<AssemblyPath>>();

    private readonly IDictionary<TreeViewItem, ConfigurationTreeViewItemMapping<AssemblyPathSelector>> _setupPathSelectorMappings
        = new Dictionary<TreeViewItem, ConfigurationTreeViewItemMapping<AssemblyPathSelector>>();

    private readonly IDictionary<TreeViewItem, ConfigurationTreeViewItemMapping<Regex>> _setupIncludePatternMappings
        = new Dictionary<TreeViewItem, ConfigurationTreeViewItemMapping<Regex>>();

    private const string TypeCodeConfigurationName = "TypeCodeConfiguration";
    private const string AssemblyRootName = "AssemblyRoot";
    private const string IncludeAssemblyPatternName = "IncludeAssemblyPattern";
    private const string AssemblyGroupName = "AssemblyGroup";
    private const string AssemblyPathName = "AssemblyPath";
    private const string AssemblyPathSelectorName = "AssemblyPathSelector";

    public SimpleConfigurator(
        IGenericXmlSerializer genericXmlSerializer,
        IConfigurationMapper configurationMapper,
        IFactory<IWizardBuilder> wizardBuilderFactory,
        IWizardRunner wizardRunner,
        IUserDataLocationProvider userDataLocationProvider
    )
    {
        _genericXmlSerializer = genericXmlSerializer;
        _configurationMapper = configurationMapper;
        _wizardBuilderFactory = wizardBuilderFactory;
        _wizardRunner = wizardRunner;
        _userDataLocationProvider = userDataLocationProvider;

        _configuration = new TypeCodeConfiguration();
    }

    public async Task<TreeViewItem> InitializeAsync()
    {
        var xmlConfiguration = await ReadXmlConfigurationAsync().ConfigureAwait(true);
        _configuration = _configurationMapper.MapToConfiguration(xmlConfiguration);
        return await BuildTreeViewAsync(_configuration).ConfigureAwait(true);
    }

    public Task AddRootAsync(TreeViewItem parentItem)
    {
        return OpenWizardAsync<AssemblyRootWizardViewModel>(model =>
        {
            if (string.IsNullOrEmpty(model.Path) || model.Priority is null)
            {
                return Task.CompletedTask;
            }

            var newItem = CreateAssemblyRootItem(model.Path, int.Parse((string)model.Priority), model.Ignore);
            parentItem.Items.Add(newItem);
            var newAssemblyRoot = new AssemblyRoot
            {
                Path = model.Path ?? throw new Exception(),
                Priority = int.Parse((string)(model.Priority ?? throw new Exception())),
                Ignore = model.Ignore
            };
            _configuration.AssemblyRoot.Add(newAssemblyRoot);
            _setupRootMappings.Add(newItem, new ConfigurationTreeViewItemMapping<AssemblyRoot>(newItem, parentItem, newAssemblyRoot));
            return Task.CompletedTask;
        }, title: "Add Root");
    }

    public bool CanAddRoot(TreeViewItem? parentItem)
    {
        return parentItem is not null && $"{parentItem.Header}".StartsWith(TypeCodeConfigurationName);
    }

    public Task AddGroupAsync(TreeViewItem parentItem)
    {
        var parentRoot = _setupRootMappings[parentItem];
        return OpenWizardAsync<AssemblyGroupWizardViewModel>(model =>
        {
            if (string.IsNullOrEmpty(model.Name) || model.Priority is null)
            {
                return Task.CompletedTask;
            }

            var newItem = CreateAssemblyGroupItem(model.Name, model.Priority.Value, model.Ignore);
            parentItem.Items.Add(newItem);
            var newAssemblyGroup = new AssemblyGroup
            {
                Name = model.Name ?? throw new Exception(),
                Priority = model.Priority ?? throw new Exception(),
                Ignore = model.Ignore
            };
            parentRoot.Type.AssemblyGroup.Add(newAssemblyGroup);
            _setupGroupMappings.Add(newItem, new ConfigurationTreeViewItemMapping<AssemblyGroup>(newItem, parentItem, newAssemblyGroup));
            return Task.CompletedTask;
        }, title: "Add Group");
    }

    public bool CanAddGroup(TreeViewItem? parentItem)
    {
        return parentItem is not null && $"{parentItem.Header}".StartsWith(AssemblyRootName);
    }

    public Task AddPathAsync(TreeViewItem parentItem)
    {
        var parentGroup = _setupGroupMappings[parentItem];
        return OpenWizardAsync<AssemblyPathWizardViewModel>(model =>
        {
            if (string.IsNullOrEmpty(model.Path) || model.Priority is null)
            {
                return Task.CompletedTask;
            }

            var newItem = CreateAssemblyPathItem(model.Priority.Value, model.Path, model.Ignore);
            parentItem.Items.Add(newItem);
            var newAssemblyPath = new AssemblyPath
            {
                Path = model.Path ?? throw new Exception(),
                Priority = model.Priority ?? throw new Exception(),
                Ignore = model.Ignore
            };
            parentGroup.Type.AssemblyPath.Add(newAssemblyPath);
            _setupPathMappings.Add(newItem, new ConfigurationTreeViewItemMapping<AssemblyPath>(newItem, parentItem, newAssemblyPath));
            return Task.CompletedTask;
        }, title: "Add Path");
    }

    public bool CanAddPath(TreeViewItem? parentItem)
    {
        return parentItem is not null && $"{parentItem.Header}".StartsWith(AssemblyGroupName);
    }

    public Task AddIncludePatternAsync(TreeViewItem parentItem)
    {
        var parentRoot = _setupRootMappings[parentItem];
        return OpenWizardAsync<IncludeAssemblyPatternWizardViewModel>(model =>
        {
            if (string.IsNullOrEmpty(model.Pattern))
            {
                return Task.CompletedTask;
            }

            var newItem = CreateIncludeAssemblyPatternItem(model.Pattern);
            parentItem.Items.Add(newItem);
            var pattern = new Regex(model.Pattern ?? string.Empty, RegexOptions.Compiled);
            parentRoot.Type.IncludeAssemblyPattern.Add(pattern);
            _setupIncludePatternMappings.Add(newItem, new ConfigurationTreeViewItemMapping<Regex>(newItem, parentItem, pattern));
            return Task.CompletedTask;
        }, title: "Add Include Pattern");
    }

    public bool CanAddIncludePattern(TreeViewItem? parentItem)
    {
        return parentItem is not null && $"{parentItem.Header}".StartsWith(AssemblyRootName);
    }

    public Task AddSelectorAsync(TreeViewItem parentItem)
    {
        var parentGroup = _setupGroupMappings[parentItem];
        return OpenWizardAsync<AssemblyPathSelectorWizardViewModel>(model =>
        {
            if (string.IsNullOrEmpty(model.Selector) || model.Priority is null)
            {
                return Task.CompletedTask;
            }

            var newItem = CreateAssemblyPathSelectorItem(model.Priority.Value, model.Selector, model.Ignore);
            parentItem.Items.Add(newItem);
            var newAssemblyPathSelector = new AssemblyPathSelector
            {
                Selector = model.Selector ?? throw new Exception(),
                Priority = model.Priority ?? throw new Exception(),
                Ignore = model.Ignore
            };
            parentGroup.Type.AssemblyPathSelector.Add(newAssemblyPathSelector);
            _setupPathSelectorMappings.Add(newItem, new ConfigurationTreeViewItemMapping<AssemblyPathSelector>(newItem, parentItem, newAssemblyPathSelector));
            return Task.CompletedTask;
        }, title: "Add Selector");
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
        await UpdateRootAsync(selectedItem).ConfigureAwait(true);
        await UpdateGroupAsync(selectedItem).ConfigureAwait(true);
        await UpdatePathAsync(selectedItem).ConfigureAwait(true);
        await UpdatePathSelectorAsync(selectedItem).ConfigureAwait(true);
        await UpdateIncludePatternAsync(selectedItem).ConfigureAwait(true);
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

    private async Task UpdateRootAsync(TreeViewItem selectedItem)
    {
        if (_setupRootMappings.ContainsKey(selectedItem))
        {
            var mapping = _setupRootMappings[selectedItem];

            await OpenWizardAsync<AssemblyRootWizardViewModel>(
                model =>
                {
                    if (string.IsNullOrEmpty(model.Path) || model.Priority is null)
                    {
                        return Task.CompletedTask;
                    }
                    
                    _setupRootMappings.Remove(selectedItem);

                    var newItem = ReplaceItemInView(mapping.ParentItem, mapping.Item, () => CreateAssemblyRootItem(model.Path, int.Parse((string)model.Priority), model.Ignore));

                    mapping.Type.Path = model.Path;
                    mapping.Type.Priority = int.Parse((string)model.Priority);
                    mapping.Type.Ignore = model.Ignore;

                    _setupRootMappings.Add(newItem, new ConfigurationTreeViewItemMapping<AssemblyRoot>(newItem, mapping.ParentItem, mapping.Type));
                    return Task.CompletedTask;
                },
                model =>
                {
                    model.Path = mapping.Type.Path;
                    model.Priority = mapping.Type.Priority.ToString();
                    model.Ignore = mapping.Type.Ignore;
                    return Task.CompletedTask;
                }, "Update", title: "Update Root").ConfigureAwait(true);
        }
    }

    private async Task UpdateGroupAsync(TreeViewItem selectedItem)
    {
        if (_setupGroupMappings.ContainsKey(selectedItem))
        {
            var mapping = _setupGroupMappings[selectedItem];

            await OpenWizardAsync<AssemblyGroupWizardViewModel>(
                model =>
                {
                    if (string.IsNullOrEmpty(model.Name) || model.Priority is null)
                    {
                        return Task.CompletedTask;
                    }
                    
                    _setupGroupMappings.Remove(selectedItem);

                    var newItem = ReplaceItemInView(mapping.ParentItem, mapping.Item, () => CreateAssemblyGroupItem(model.Name, model.Priority.Value, model.Ignore));

                    mapping.Type.Name = model.Name;
                    mapping.Type.Priority = model.Priority.Value;
                    mapping.Type.Ignore = model.Ignore;

                    _setupGroupMappings.Add(newItem, new ConfigurationTreeViewItemMapping<AssemblyGroup>(newItem, mapping.ParentItem, mapping.Type));
                    return Task.CompletedTask;
                },
                model =>
                {
                    model.Name = mapping.Type.Name;
                    model.Priority = mapping.Type.Priority;
                    model.Ignore = mapping.Type.Ignore;
                    return Task.CompletedTask;
                }, "Update", title: "Update Group").ConfigureAwait(true);
        }
    }

    private async Task UpdatePathAsync(TreeViewItem selectedItem)
    {
        if (_setupPathMappings.ContainsKey(selectedItem))
        {
            var mapping = _setupPathMappings[selectedItem];

            await OpenWizardAsync<AssemblyPathWizardViewModel>(
                model =>
                {
                    if (string.IsNullOrEmpty(model.Path) || model.Priority is null)
                    {
                        return Task.CompletedTask;
                    }
                    
                    _setupPathMappings.Remove(selectedItem);
                    
                    var newItem = ReplaceItemInView(mapping.ParentItem, mapping.Item, () => CreateAssemblyPathItem(model.Priority.Value, model.Path, model.Ignore));

                    mapping.Type.Path = model.Path;
                    mapping.Type.Priority = model.Priority.Value;
                    mapping.Type.Ignore = model.Ignore;

                    _setupPathMappings.Add(newItem, new ConfigurationTreeViewItemMapping<AssemblyPath>(newItem, mapping.ParentItem, mapping.Type));
                    return Task.CompletedTask;
                },
                model =>
                {
                    model.Path = mapping.Type.Path;
                    model.Priority = mapping.Type.Priority;
                    model.Ignore = mapping.Type.Ignore;
                    return Task.CompletedTask;
                }, "Update", title: "Update Path").ConfigureAwait(true);
        }
    }

    private async Task UpdatePathSelectorAsync(TreeViewItem selectedItem)
    {
        if (_setupPathSelectorMappings.ContainsKey(selectedItem))
        {
            var mapping = _setupPathSelectorMappings[selectedItem];

            await OpenWizardAsync<AssemblyPathSelectorWizardViewModel>(
                model =>
                {
                    if (string.IsNullOrEmpty(model.Selector) || model.Priority is null)
                    {
                        return Task.CompletedTask;
                    }
                    
                    _setupPathSelectorMappings.Remove(selectedItem);
                    
                    var newItem = ReplaceItemInView(mapping.ParentItem, mapping.Item, () => CreateAssemblyPathSelectorItem(model.Priority.Value, model.Selector, model.Ignore));

                    mapping.Type.Selector = model.Selector;
                    mapping.Type.Priority = model.Priority.Value;
                    mapping.Type.Ignore = model.Ignore;

                    _setupPathSelectorMappings.Add(newItem, new ConfigurationTreeViewItemMapping<AssemblyPathSelector>(newItem, mapping.ParentItem, mapping.Type));
                    return Task.CompletedTask;
                },
                model =>
                {
                    model.Selector = mapping.Type.Selector;
                    model.Priority = mapping.Type.Priority;
                    model.Ignore = mapping.Type.Ignore;
                    return Task.CompletedTask;
                }, "Update", title: "Update Path Selector").ConfigureAwait(true);
        }
    }

    private async Task UpdateIncludePatternAsync(TreeViewItem selectedItem)
    {
        if (_setupIncludePatternMappings.ContainsKey(selectedItem))
        {
            var mapping = _setupIncludePatternMappings[selectedItem];

            await OpenWizardAsync<IncludeAssemblyPatternWizardViewModel>(
                model =>
                {
                    if (string.IsNullOrEmpty(model.Pattern))
                    {
                        return Task.CompletedTask;
                    }
                    
                    _setupIncludePatternMappings.Remove(selectedItem);
                    var newItem = ReplaceItemInView(mapping.ParentItem, mapping.Item, () => CreateIncludeAssemblyPatternItem(model.Pattern));
                    _setupIncludePatternMappings.Add(newItem, new ConfigurationTreeViewItemMapping<Regex>(newItem, mapping.ParentItem, new Regex(model.Pattern ?? string.Empty, RegexOptions.Compiled)));
                    return Task.CompletedTask;
                },
                model =>
                {
                    model.Pattern = $"{mapping.Type}";
                    return Task.CompletedTask;
                }, "Update", title: "Update Include Assembly Pattern").ConfigureAwait(true);
        }
    }

    private static TreeViewItem ReplaceItemInView(ItemsControl parentItem, ItemsControl item, Func<TreeViewItem> createNewItem)
    {
        var oldIndex = parentItem.Items.IndexOf(item);
        parentItem.Items.Remove(item);

        var newItem = createNewItem();

        var subItemsToRemove = new List<object?>();
        var subItemsToAdd = new List<object?>();
        foreach (var subItem in item.Items)
        {
            subItemsToRemove.Add(subItem);
            subItemsToAdd.Add(subItem);
        }

        foreach (var subItem in subItemsToRemove)
        {
            item.Items.Remove(subItem);
        }

        foreach (var subItem in subItemsToAdd)
        {
            newItem.Items.Add(subItem);
        }

        parentItem.Items.Insert(oldIndex, newItem);
        return newItem;
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
            var assemblyRootItem = CreateAssemblyRootItem(assemblyRoot.Path, assemblyRoot.Priority, assemblyRoot.Ignore);
            BuildIncludeAssemblyPatterns(assemblyRoot, assemblyRootItem);
            BuildAssemblyGroups(assemblyRoot, assemblyRootItem);
            typeCodeConfigurationItem.Items.Add(assemblyRootItem);
            _setupRootMappings.Add(assemblyRootItem, new ConfigurationTreeViewItemMapping<AssemblyRoot>(assemblyRootItem, typeCodeConfigurationItem, assemblyRoot));
        }
    }

    private void BuildAssemblyGroups(AssemblyRoot assemblyRoot, TreeViewItem assemblyRootItem)
    {
        foreach (var assemblyGroup in assemblyRoot.AssemblyGroup)
        {
            var assemblyGroupItem = CreateAssemblyGroupItem(assemblyGroup.Name, assemblyGroup.Priority, assemblyGroup.Ignore);
            BuildAssemblyPaths(assemblyGroup, assemblyGroupItem);
            BuildAssemblyPathSelectors(assemblyGroup, assemblyGroupItem);
            assemblyRootItem.Items.Add(assemblyGroupItem);
            _setupGroupMappings.Add(assemblyGroupItem, new ConfigurationTreeViewItemMapping<AssemblyGroup>(assemblyGroupItem, assemblyRootItem, assemblyGroup));
        }
    }

    private void BuildIncludeAssemblyPatterns(AssemblyRoot assemblyRoot, TreeViewItem assemblyRootItem)
    {
        foreach (var includeAssemblyPattern in assemblyRoot.IncludeAssemblyPattern)
        {
            var includeAssemblyPatternItem = CreateIncludeAssemblyPatternItem($"{includeAssemblyPattern}");
            assemblyRootItem.Items.Add(includeAssemblyPatternItem);
            _setupIncludePatternMappings.Add(includeAssemblyPatternItem, new ConfigurationTreeViewItemMapping<Regex>(includeAssemblyPatternItem, assemblyRootItem, includeAssemblyPattern));
        }
    }

    private void BuildAssemblyPaths(AssemblyGroup assemblyGroup, TreeViewItem assemblyGroupItem)
    {
        foreach (var assemblyPath in assemblyGroup.AssemblyPath)
        {
            var assemblyPathItem = CreateAssemblyPathItem(assemblyPath.Priority, assemblyPath.Path, assemblyPath.Ignore);
            assemblyGroupItem.Items.Add(assemblyPathItem);
            _setupPathMappings.Add(assemblyPathItem, new ConfigurationTreeViewItemMapping<AssemblyPath>(assemblyPathItem, assemblyGroupItem, assemblyPath));
        }
    }

    private void BuildAssemblyPathSelectors(AssemblyGroup assemblyGroup, TreeViewItem assemblyGroupItem)
    {
        foreach (var assemblyPathSelector in assemblyGroup.AssemblyPathSelector)
        {
            var assemblyPathSelectorItem = CreateAssemblyPathSelectorItem(assemblyPathSelector.Priority, assemblyPathSelector.Selector, assemblyPathSelector.Ignore);
            assemblyGroupItem.Items.Add(assemblyPathSelectorItem);
            _setupPathSelectorMappings.Add(assemblyPathSelectorItem, new ConfigurationTreeViewItemMapping<AssemblyPathSelector>(assemblyPathSelectorItem, assemblyGroupItem, assemblyPathSelector));
        }
    }

    private static TreeViewItem CreateAssemblyRootItem(string path, int priority, bool ignore)
    {
        return new TreeViewItem
        {
            Header = $"{AssemblyRootName} Path={path} Priority={priority} Ignore={ignore}",
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

    private static TreeViewItem CreateAssemblyGroupItem(string name, int priority, bool ignore)
    {
        return new TreeViewItem
        {
            Header = $"{AssemblyGroupName} Name={name} Priority={priority} Ignore={ignore}",
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

    private static TreeViewItem CreateAssemblyPathItem(int priority, string path, bool ignore)
    {
        return new TreeViewItem
        {
            Header = $"{AssemblyPathName} Priority={priority} Value={path} Ignore={ignore}",
            IsExpanded = true
        };
    }

    private static TreeViewItem CreateAssemblyPathSelectorItem(int priority, string selector, bool ignore)
    {
        return new TreeViewItem
        {
            Header = $"{AssemblyPathSelectorName} Priority={priority} Value={selector} Ignore={ignore}",
            IsExpanded = true
        };
    }

    private Task OpenWizardAsync<T>(Func<T, Task>? completed = null, Func<T, Task>? initialize = null, string? finishButtonText = null, string? title = null) where T : notnull
    {
        var wizard = _wizardBuilderFactory.Create()
            .Then<T>((options, _) => options.Title(title).Before(c => initialize is not null ? initialize(c.GetParameter<T>()) : Task.CompletedTask))
            .FinishAsync(c => completed is not null ? completed(c.GetParameter<T>()) : Task.CompletedTask, finishButtonText ?? "Add")
            .Build();

        return _wizardRunner.RunAsync(wizard);
    }

    private async Task<XmlTypeCodeConfiguration> ReadXmlConfigurationAsync()
    {
        var cfg = _userDataLocationProvider.GetConfigurationFilePath();
        var xml = await File.ReadAllTextAsync(cfg).ConfigureAwait(true);
        return _genericXmlSerializer.Deserialize<XmlTypeCodeConfiguration>(xml) ?? throw new Exception($"{cfg} can not be parsed");
    }

    private Task WriteXmlConfigurationAsync(XmlTypeCodeConfiguration xmlConfiguration)
    {
        var cfg = _userDataLocationProvider.GetConfigurationFilePath();
        var serialized = _genericXmlSerializer.Serialize(xmlConfiguration);
        return File.WriteAllTextAsync(cfg, serialized);
    }
}